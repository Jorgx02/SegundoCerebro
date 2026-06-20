namespace SegundoCerebro.Application.DTOs.Auth;

/// <summary>
/// DTO utilizado para eliminar la cuenta de un usuario.
/// </summary>
public class DeleteAccountDto
{
    public string Password { get; set; } = string.Empty;
}