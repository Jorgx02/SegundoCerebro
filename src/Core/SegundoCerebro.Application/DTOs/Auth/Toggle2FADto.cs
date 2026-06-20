namespace SegundoCerebro.Application.DTOs.Auth;

/// <summary>
/// DTO para habilitar o deshabilitar la autenticación de dos factores (2FA) para un usuario.
/// </summary>
public class Toggle2FADto
{
    public string Password { get; set; } = string.Empty;
    public bool Enable { get; set; }
}