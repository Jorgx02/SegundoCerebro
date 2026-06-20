namespace SegundoCerebro.Application.DTOs.Auth;

/// <summary>
/// DTO de respuesta para la acción de habilitar/deshabilitar la autenticación de dos factores (2FA).
/// </summary>
public class Toggle2FAResponseDto
{
    public bool RequiresCode { get; set; }
    public string Message { get; set; } = string.Empty;
}