using System.Net.Http.Json;
using SegundoCerebro.BlazorWasm.Models;

namespace SegundoCerebro.BlazorWasm.Services;

public class WellnessService : IWellnessService
{
    private readonly HttpClient _httpClient;

    public WellnessService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<WellnessEntryDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<WellnessEntryDto>>($"api/wellness?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
        return result ?? Enumerable.Empty<WellnessEntryDto>();
    }

    public async Task<WellnessEntryDto> UpsertAsync(UpsertWellnessEntryDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/wellness", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<WellnessEntryDto>() ?? throw new Exception("Failed to deserialize response.");
    }
}