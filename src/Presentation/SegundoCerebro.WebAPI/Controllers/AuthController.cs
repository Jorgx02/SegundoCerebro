using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SegundoCerebro.Application.DTOs.Auth;

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
}
