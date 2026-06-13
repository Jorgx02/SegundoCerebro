namespace SegundoCerebro.Application.DTOs.Auth; // O SegundoCerebro.BlazorWasm.Models.Auth para el frontend

public class ResetPasswordDto
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}