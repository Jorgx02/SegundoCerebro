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
}