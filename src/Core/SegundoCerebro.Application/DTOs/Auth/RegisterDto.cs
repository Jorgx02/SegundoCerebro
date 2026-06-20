namespace SegundoCerebro.Application.DTOs.Auth;

/// <summary>
/// DTO de transferencia de datos utilizado para registrar un nuevo usuario en el sistema.
/// </summary>
public class RegisterDto
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}