namespace SegundoCerebro.BlazorWasm.Models.Auth;

public class Toggle2FAResponseDto
{
    public bool RequiresCode { get; set; }
    public string Message { get; set; } = string.Empty;
}