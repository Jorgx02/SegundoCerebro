using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using SegundoCerebro.Application.DTOs.Auth;
using SegundoCerebro.Infrastructure.Data;

namespace SegundoCerebro.WebAPI.Controllers;

/// <summary>
/// Controlador principal para gestionar la autenticación de usuarios.
/// Proporciona endpoints para el registro de cuentas y el inicio de sesión.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    /// <summary>
    /// Endpoint para registrar un nuevo usuario en la base de datos utilizando ASP.NET Core Identity.
    /// </summary>
    /// <param name="request">Credenciales del nuevo usuario (Email y Password).</param>
    /// <returns>Un mensaje de éxito o los errores de validación de la contraseña.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        var user = new IdentityUser { UserName = request.Email, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new { message = "Usuario registrado correctamente." });
    }

    /// <summary>
    /// Endpoint para iniciar sesión. Valida las credenciales y devuelve un Token JWT válido por 2 horas.
    /// </summary>
    /// <param name="request">Credenciales del usuario.</param>
    /// <returns>Un AuthResponseDto con el Token JWT generado.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return Unauthorized(new { message = "Email o contraseña incorrectos." });
        }

        var jwtSettings = _configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "TuClaveSuperSecretaDeDesarrolloParaJWT123456789");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim("username", user.UserName ?? user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2), // Tiempo de expiración del token
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Ok(new AuthResponseDto { Token = tokenHandler.WriteToken(token), Email = user.Email! });
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        if (user.Email != request.Email)
        {
            var existing = await _userManager.FindByEmailAsync(request.Email);
            if (existing != null && existing.Id != user.Id) return BadRequest(new { message = "El email ya está en uso." });
            user.Email = request.Email;
        }

        user.UserName = request.UserName;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) return BadRequest(result.Errors);
        return Ok(new { message = "Perfil actualizado." });
    }

    [Authorize]
    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId!);
        if (user == null) return NotFound();

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded) return BadRequest(result.Errors);
        return Ok(new { message = "Contraseña actualizada." });
    }

    [Authorize]
    [HttpPost("delete-account")]
    public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountDto request, [FromServices] ApplicationDbContext dbContext)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId!);
        if (user == null) return NotFound();

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
            return Unauthorized(new { message = "Contraseña incorrecta." });

        // Eliminamos de forma segura los datos financieros asociados antes de borrar al usuario
        dbContext.Budgets.RemoveRange(dbContext.Budgets.Where(b => b.UserId == userId));
        dbContext.Transactions.RemoveRange(dbContext.Transactions.Where(t => t.UserId == userId));
        dbContext.Accounts.RemoveRange(dbContext.Accounts.Where(a => a.UserId == userId));
        await dbContext.SaveChangesAsync();

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded) return BadRequest(result.Errors);
        return Ok(new { message = "Cuenta eliminada." });
    }
}
