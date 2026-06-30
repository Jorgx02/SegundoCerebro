using System.Net.Http.Json;
using SegundoCerebro.BlazorWasm.Models;
using System.Text.Json;

namespace SegundoCerebro.BlazorWasm.Services;

/// <summary>
/// Implementación del servicio para gestionar hábitos a través de la API.
/// </summary>
public class HabitService : ApiService<HabitDto, CreateHabitDto, UpdateHabitDto>, IHabitService
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="HabitService"/>.
    /// </summary>
    public HabitService(HttpClient httpClient) : base(httpClient, "habits")
    {
    }

    /// <inheritdoc />
    public async Task<IEnumerable<HabitDto>> GetHabitsForTrackerAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<HabitDto>>($"api/{_endpoint}/tracker");
        return result ?? Enumerable.Empty<HabitDto>();
    }

    /// <inheritdoc />
    public async Task<bool> ToggleHabitCompletionAsync(Guid habitId, DateTime date)
    {
        var command = new { Date = date };
        var response = await _httpClient.PostAsJsonAsync($"api/{_endpoint}/{habitId}/toggle", command);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<bool>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}