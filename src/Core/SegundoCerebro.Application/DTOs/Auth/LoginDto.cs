namespace SegundoCerebro.Application.DTOs.Auth;

/// <summary>
/// DTO utilizado para que un usuario inicie sesión.
/// </summary>
public class LoginDto
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}