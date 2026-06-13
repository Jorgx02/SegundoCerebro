using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        if (user.TwoFactorEnabled)
        {
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            var userName = user.UserName ?? "Usuario";
            var emailBody = $@"
            <div style='font-family: Arial, sans-serif; background-color: #f4f6f8; padding: 40px 20px; color: #333;'>
                <div style='max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1);'>
                    <div style='background-color: #512BD4; padding: 20px; text-align: center;'>
                        <h1 style='color: #ffffff; margin: 0; font-size: 24px; font-weight: 700;'>SegundoCerebro</h1>
                    </div>
                    <div style='padding: 30px;'>
                        <h2 style='color: #333; font-size: 20px; margin-top: 0;'>Código de Verificación</h2>
                        <p style='font-size: 16px; color: #555;'>Hola <strong>{userName}</strong>,</p>
                        <p style='font-size: 16px; color: #555; line-height: 1.5;'>Estás intentando iniciar sesión en tu cuenta. Por favor, utiliza el siguiente código de verificación de 6 dígitos para completar el proceso:</p>
                        
                        <div style='text-align: center; margin: 30px 0; background-color: #f8f9fa; padding: 20px; border-radius: 8px; border: 2px dashed #512BD4;'>
                            <span style='font-size: 32px; font-weight: bold; color: #512BD4; letter-spacing: 5px;'>{code}</span>
                        </div>
                        
                        <p style='font-size: 14px; color: #777; border-top: 1px solid #eee; padding-top: 20px;'>Este código expira en 10 minutos. Si no has intentado iniciar sesión, te recomendamos que cambies tu contraseña inmediatamente.</p>
                    </div>
                </div>
            </div>";

            await SendEmailAsync(user.Email!, "Tu código de acceso (2FA)", emailBody);
            return Ok(new AuthResponseDto { Requires2FA = true, Email = user.Email! });
        }

        var token = GenerateJwtToken(user);
        return Ok(new AuthResponseDto { Token = token, Email = user.Email! });
    }

    [HttpPost("verify-2fa")]
    public async Task<IActionResult> Verify2FALogin([FromBody] Verify2FALoginDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) return Unauthorized();

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", request.Code);
        if (!isValid) return Unauthorized(new { message = "Código incorrecto o expirado." });

        var token = GenerateJwtToken(user);
        return Ok(new AuthResponseDto { Token = token, Email = user.Email! });
    }

    private string GenerateJwtToken(IdentityUser user)
    {
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
        var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(jwtToken);
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

    [Authorize]
    [HttpGet("2fa-status")]
    public async Task<IActionResult> Get2FAStatus()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId!);
        if (user == null) return NotFound();
        return Ok(new { isEnabled = user.TwoFactorEnabled });
    }

    [Authorize]
    [HttpPost("toggle-2fa")]
    public async Task<IActionResult> Toggle2FA([FromBody] Toggle2FADto request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId!);
        if (user == null) return NotFound();
        if (!await _userManager.CheckPasswordAsync(user, request.Password)) return Unauthorized(new { message = "Contraseña incorrecta." });

        if (request.Enable)
        {
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            var userName = user.UserName ?? "Usuario";
            var emailBody = $@"
            <div style='font-family: Arial, sans-serif; background-color: #f4f6f8; padding: 40px 20px; color: #333;'>
                <div style='max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1);'>
                    <div style='background-color: #512BD4; padding: 20px; text-align: center;'>
                        <h1 style='color: #ffffff; margin: 0; font-size: 24px; font-weight: 700;'>SegundoCerebro</h1>
                    </div>
                    <div style='padding: 30px;'>
                        <h2 style='color: #333; font-size: 20px; margin-top: 0;'>Activación de 2FA</h2>
                        <p style='font-size: 16px; color: #555;'>Hola <strong>{userName}</strong>,</p>
                        <p style='font-size: 16px; color: #555; line-height: 1.5;'>Estás a un paso de añadir una capa extra de seguridad a tu cuenta. Utiliza el siguiente código para confirmar la activación de la Autenticación de Dos Factores:</p>
                        
                        <div style='text-align: center; margin: 30px 0; background-color: #f8f9fa; padding: 20px; border-radius: 8px; border: 2px dashed #512BD4;'>
                            <span style='font-size: 32px; font-weight: bold; color: #512BD4; letter-spacing: 5px;'>{code}</span>
                        </div>
                        
                        <p style='font-size: 14px; color: #777; border-top: 1px solid #eee; padding-top: 20px;'>Si no solicitaste activar el 2FA, puedes ignorar este correo de forma segura.</p>
                    </div>
                </div>
            </div>";

            await SendEmailAsync(user.Email!, "Código de Activación 2FA", emailBody);
            return Ok(new Toggle2FAResponseDto { RequiresCode = true, Message = "Se ha enviado un código de verificación." });
        }
        else
        {
            await _userManager.SetTwoFactorEnabledAsync(user, false);
            return Ok(new Toggle2FAResponseDto { RequiresCode = false, Message = "2FA desactivado correctamente." });
        }
    }

    [Authorize]
    [HttpPost("confirm-2fa")]
    public async Task<IActionResult> Confirm2FA([FromBody] Confirm2FADto request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId!);

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user!, "Email", request.Code);
        if (!isValid) return BadRequest(new { message = "Código incorrecto o expirado." });

        await _userManager.SetTwoFactorEnabledAsync(user!, true);
        return Ok(new { message = "2FA activado exitosamente." });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        // Por seguridad, siempre devolvemos Ok para no revelar si el correo existe o no a un atacante
        if (user == null) return Ok(new { message = "Si el correo existe, se ha enviado un enlace." });

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetLink = $"http://localhost:5173/reset-password?email={user.Email}&token={Uri.EscapeDataString(token)}";

        var userName = user.UserName ?? "Usuario";

        var emailBody = $@"
        <div style='font-family: Arial, sans-serif; background-color: #f4f6f8; padding: 40px 20px; color: #333;'>
            <div style='max-width: 600px; margin: 0 auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1);'>
                <div style='background-color: #512BD4; padding: 20px; text-align: center;'>
                    <h1 style='color: #ffffff; margin: 0; font-size: 24px; font-weight: 700;'>SegundoCerebro</h1>
                </div>
                <div style='padding: 30px;'>
                    <h2 style='color: #333; font-size: 20px; margin-top: 0;'>Recuperación de Contraseña</h2>
                    <p style='font-size: 16px; color: #555;'>Hola <strong>{userName}</strong>,</p>
                    <p style='font-size: 16px; color: #555; line-height: 1.5;'>Hemos recibido una solicitud para restablecer la contraseña de tu cuenta en SegundoCerebro.</p>
                    <p style='font-size: 16px; color: #555; line-height: 1.5;'>Haz clic en el siguiente botón para crear una nueva contraseña:</p>
                    
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{resetLink}' style='background-color: #512BD4; color: #ffffff; padding: 12px 24px; text-decoration: none; border-radius: 6px; font-weight: bold; font-size: 16px; display: inline-block;'>Restablecer Contraseña</a>
                    </div>
                    
                    <p style='font-size: 14px; color: #777; margin-bottom: 5px;'>O copia y pega el siguiente enlace en tu navegador:</p>
                    <p style='font-size: 14px; word-break: break-all; margin-top: 0;'><a href='{resetLink}' style='color: #512BD4;'>{resetLink}</a></p>
                    
                    <div style='background-color: #fff3cd; border-left: 4px solid #ffc107; padding: 15px; margin: 30px 0; border-radius: 4px;'>
                        <h3 style='margin-top: 0; color: #856404; font-size: 16px;'>⚠️ Importante:</h3>
                        <ul style='color: #856404; font-size: 14px; padding-left: 20px; margin-bottom: 0; line-height: 1.6;'>
                            <li>Este enlace expira en 1 hora</li>
                            <li>Solo puedes usar este enlace una vez</li>
                            <li>Si no solicitaste este cambio, ignora este email</li>
                            <li>Tu contraseña no cambiará hasta que accedas al enlace y establezcas una nueva</li>
                        </ul>
                    </div>
                    
                    <p style='font-size: 14px; color: #777; border-top: 1px solid #eee; padding-top: 20px;'>Si no solicitaste restablecer tu contraseña, puedes ignorar este mensaje de forma segura.</p>
                </div>
            </div>
        </div>";

        await SendEmailAsync(user.Email!, "Recuperación de Contraseña", emailBody);
        return Ok(new { message = "Si el correo existe, se ha enviado un enlace." });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) return BadRequest(new { message = "Error al restablecer la contraseña." });

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (!result.Succeeded) return BadRequest(result.Errors);
        return Ok(new { message = "Contraseña restablecida." });
    }

    private async Task SendEmailAsync(string to, string subject, string body)
    {
        var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
        {
            Credentials = new NetworkCredential("b6015083cfd2e9", "ec4019b88a4ce6"),
            EnableSsl = true
        };
        var mailMessage = new MailMessage { From = new MailAddress("noreply@segundocerebro.com", "SegundoCerebro"), Subject = subject, Body = body, IsBodyHtml = true };
        mailMessage.To.Add(to);
        await client.SendMailAsync(mailMessage);
    }
}
