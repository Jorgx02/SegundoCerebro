using System.Net.Http.Json;
using SegundoCerebro.BlazorWasm.Models;
using SegundoCerebro.BlazorWasm.Models.Commands;

namespace SegundoCerebro.BlazorWasm.Services;

public class CardService : ICardService
{
    private readonly HttpClient _httpClient;

    public CardService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<CardDto>> GetByAccountIdAsync(Guid accountId)
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<CardDto>>($"api/cards/account/{accountId}");
        return result ?? Enumerable.Empty<CardDto>();
    }

    public async Task<CardDto> CreateAsync(CreateCardCommand command)
    {
        var response = await _httpClient.PostAsJsonAsync("api/cards", command);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CardDto>() ?? throw new Exception("Could not deserialize card from API response.");
    }

    public async Task DeleteAsync(Guid cardId)
    {
        var response = await _httpClient.DeleteAsync($"api/cards/{cardId}");
        response.EnsureSuccessStatusCode();
    }
}