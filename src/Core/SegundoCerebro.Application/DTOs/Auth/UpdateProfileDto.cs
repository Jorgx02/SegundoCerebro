namespace SegundoCerebro.Application.DTOs.Auth;

/// <summary>
/// DTO para actualizar el perfil de usuario.
/// </summary>
public class UpdateProfileDto
{
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}