namespace SegundoCerebro.Application.DTOs.Auth;

/// <summary>
/// DTO utilizado para cambiar la contraseña de un usuario.
/// </summary>
public class ChangePasswordDto
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}