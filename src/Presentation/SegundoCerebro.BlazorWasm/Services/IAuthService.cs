using SegundoCerebro.BlazorWasm.Models.Auth;

namespace SegundoCerebro.BlazorWasm.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task LogoutAsync();
    Task<bool> UpdateProfileAsync(UpdateProfileDto dto);
    Task<bool> ChangePasswordAsync(ChangePasswordDto dto);
    Task<bool> DeleteAccountAsync(DeleteAccountDto dto);
}