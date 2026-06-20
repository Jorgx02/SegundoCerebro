namespace SegundoCerebro.Application.DTOs.Auth;

/// <summary>
/// DTO para confirmar la autenticación de dos factores (2FA).
/// </summary>
public class Confirm2FADto
{
    public string Code { get; set; } = string.Empty;
}