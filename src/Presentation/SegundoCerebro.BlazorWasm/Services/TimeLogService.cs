using System.Net.Http.Json;
using SegundoCerebro.BlazorWasm.Models;
using SegundoCerebro.BlazorWasm.Models.Commands;

namespace SegundoCerebro.BlazorWasm.Services;

public class TimeLogService : ITimeLogService
{
    private readonly HttpClient _httpClient;

    public TimeLogService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TimeLogDto> StartAsync(Guid todoItemId)
    {
        var command = new StartTimeLogCommand { TodoItemId = todoItemId };
        var response = await _httpClient.PostAsJsonAsync("api/timelogs/start", command);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<TimeLogDto>())!;
    }

    public async Task<TimeLogDto> StopAsync(Guid todoItemId)
    {
        var command = new StopTimeLogCommand { TodoItemId = todoItemId };
        var response = await _httpClient.PostAsJsonAsync("api/timelogs/stop", command);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<TimeLogDto>())!;
    }
}