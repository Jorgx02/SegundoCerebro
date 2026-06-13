namespace SegundoCerebro.BlazorWasm.Models.Auth;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Requires2FA { get; set; }
}
