using SegundoCerebro.BlazorWasm.Models.Auth;

namespace SegundoCerebro.BlazorWasm.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto> Verify2FALoginAsync(Verify2FALoginDto dto);
    Task LogoutAsync();
    Task<bool> UpdateProfileAsync(UpdateProfileDto dto);
    Task<bool> ChangePasswordAsync(ChangePasswordDto dto);
    Task<bool> DeleteAccountAsync(DeleteAccountDto dto);
    Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto);
    Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
    Task<bool> Get2FAStatusAsync();
    Task<Toggle2FAResponseDto> Toggle2FAAsync(Toggle2FADto dto);
    Task<bool> Confirm2FAAsync(Confirm2FADto dto);
}