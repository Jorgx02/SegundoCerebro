namespace SegundoCerebro.BlazorWasm.Models.Auth;

public class Toggle2FADto
{
    public string Password { get; set; } = string.Empty;
    public bool Enable { get; set; }
}