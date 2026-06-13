namespace SegundoCerebro.Application.DTOs.Auth;

/// <summary>
/// Respuesta devuelta por la API tras un inicio de sesión exitoso.
/// Contiene el token JWT necesario para autenticar futuras peticiones.
/// </summary>
public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool Requires2FA { get; set; }
}