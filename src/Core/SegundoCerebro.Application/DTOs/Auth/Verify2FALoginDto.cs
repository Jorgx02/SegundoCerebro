namespace SegundoCerebro.Application.DTOs.Auth;

/// <summary>
/// DTO para verificar el inicio de sesión con 2FA.
/// </summary>
public class Verify2FALoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}