namespace SegundoCerebro.BlazorWasm.Models.Auth;

public class Verify2FALoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}