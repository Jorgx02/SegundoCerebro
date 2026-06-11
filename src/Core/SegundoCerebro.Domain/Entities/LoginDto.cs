namespace SegundoCerebro.Application.DTOs.Auth;

/// <summary>
/// Objeto de transferencia de datos utilizado para que un usuario inicie sesión.
/// </summary>
public class LoginDto
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}