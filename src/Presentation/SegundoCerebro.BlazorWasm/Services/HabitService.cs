using System.Net.Http.Json;
using SegundoCerebro.BlazorWasm.Models;

namespace SegundoCerebro.BlazorWasm.Services;

public class HabitService : ApiService<HabitDto, CreateHabitDto, UpdateHabitDto>, IHabitService
{
    public HabitService(HttpClient httpClient) : base(httpClient, "habits")
    {
    }

    public async Task<IEnumerable<HabitDto>> GetHabitsForTrackerAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<HabitDto>>("api/habits/tracker");
        return result ?? Enumerable.Empty<HabitDto>();
    }

    public async Task<bool> ToggleHabitCompletionAsync(Guid habitId, DateTime date)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/habits/{habitId}/toggle", new { date });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<bool>();
    }

    public async Task<IEnumerable<HabitHeatmapDto>> GetHeatmapDataAsync(int year)
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<HabitHeatmapDto>>($"api/habits/heatmap/{year}");
        return result ?? Enumerable.Empty<HabitHeatmapDto>();
    }

    public async Task<HabitStatsDto> GetHabitStatsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<HabitStatsDto>("api/habits/stats");
        return result ?? new HabitStatsDto();
    }
}

// Se asume la existencia de ApiService<T, TCreate, TUpdate>