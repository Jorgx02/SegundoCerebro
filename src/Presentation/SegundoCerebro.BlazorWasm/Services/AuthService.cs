using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SegundoCerebro.BlazorWasm.Models.Auth;
using SegundoCerebro.BlazorWasm.Providers;

namespace SegundoCerebro.BlazorWasm.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
    }

    public async Task<bool> RegisterAsync(RegisterDto registerDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerDto);
        return response.IsSuccessStatusCode;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);

        if (!response.IsSuccessStatusCode)
            throw new Exception("Login failed");

        var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>(options);

        if (result!.Requires2FA) return result;

        if (string.IsNullOrWhiteSpace(result.Token))
            throw new Exception("Error al recibir el Token de sesión.");

        await _localStorage.SetItemAsync("authToken", result!.Token);
        ((CustomAuthenticationStateProvider)_authStateProvider).MarkUserAsAuthenticated(result.Token);

        return result;
    }

    public async Task<AuthResponseDto> Verify2FALoginAsync(Verify2FALoginDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/verify-2fa", dto);
        if (!response.IsSuccessStatusCode) throw new Exception("Verification failed");
        var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        await _localStorage.SetItemAsync("authToken", result!.Token);
        ((CustomAuthenticationStateProvider)_authStateProvider).MarkUserAsAuthenticated(result.Token);
        return result;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        ((CustomAuthenticationStateProvider)_authStateProvider).MarkUserAsLoggedOut();
    }

    public async Task<bool> UpdateProfileAsync(UpdateProfileDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync("api/auth/profile", dto);
        if (!response.IsSuccessStatusCode) throw new Exception("Error updating profile");
        return true;
    }

    public async Task<bool> ChangePasswordAsync(ChangePasswordDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync("api/auth/password", dto);
        if (!response.IsSuccessStatusCode) throw new Exception("Error changing password");
        return true;
    }

    public async Task<bool> DeleteAccountAsync(DeleteAccountDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/delete-account", dto);
        if (!response.IsSuccessStatusCode) throw new Exception("Error deleting account");
        return true;
    }

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/forgot-password", dto);
        if (!response.IsSuccessStatusCode) throw new Exception("Error sending email");
        return true;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/reset-password", dto);
        if (!response.IsSuccessStatusCode) throw new Exception("Error resetting password");
        return true;
    }

    public class TwoFactorStatusResponse { public bool isEnabled { get; set; } }

    public async Task<bool> Get2FAStatusAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/auth/2fa-status");
            if (!response.IsSuccessStatusCode) return false;

            var result = await response.Content.ReadFromJsonAsync<TwoFactorStatusResponse>();
            return result?.isEnabled ?? false;
        }
        catch { return false; }
    }

    public async Task<Toggle2FAResponseDto> Toggle2FAAsync(Toggle2FADto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/toggle-2fa", dto);
        if (!response.IsSuccessStatusCode) throw new Exception("Error toggling 2FA");
        return await response.Content.ReadFromJsonAsync<Toggle2FAResponseDto>() ?? new Toggle2FAResponseDto();
    }

    public async Task<bool> Confirm2FAAsync(Confirm2FADto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/confirm-2fa", dto);
        if (!response.IsSuccessStatusCode) throw new Exception("Error confirming 2FA");
        return true;
    }
}