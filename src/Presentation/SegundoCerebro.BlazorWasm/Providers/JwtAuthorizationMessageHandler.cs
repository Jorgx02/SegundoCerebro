using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using SegundoCerebro.BlazorWasm.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SegundoCerebro.BlazorWasm.Providers;

/// <summary>
/// Intercepta cada solicitud HTTP para añadir el token JWT y manejar las respuestas de autorización.
/// </summary>
public class JwtAuthorizationMessageHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;
    private readonly IServiceProvider _serviceProvider;

    public JwtAuthorizationMessageHandler(ILocalStorageService localStorage, IServiceProvider serviceProvider)
    {
        _localStorage = localStorage;
        _serviceProvider = serviceProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Intentar obtener el token del almacenamiento local
        var token = await _localStorage.GetItemAsync<string>("authToken", cancellationToken);

        // Si el token existe, añadirlo a la cabecera de autorización
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Enviar la solicitud a la API
        var response = await base.SendAsync(request, cancellationToken);

        // Si la respuesta es 401 Unauthorized (token expirado o inválido)
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            // Usamos el ServiceProvider para evitar una dependencia circular
            // con IAuthService, que a su vez depende de HttpClient.
            var authService = _serviceProvider.GetRequiredService<IAuthService>();
            var navigationManager = _serviceProvider.GetRequiredService<NavigationManager>();

            // Limpiar la sesión local y el estado de autenticación
            await authService.LogoutAsync();

            // Redirigir al usuario a la página de login
            navigationManager.NavigateTo("/login");
        }

        return response;
    }
}
