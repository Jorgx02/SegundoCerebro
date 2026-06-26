using System.Net.Http.Json;
using SegundoCerebro.BlazorWasm.Models;
using SegundoCerebro.BlazorWasm.Models.Commands;

namespace SegundoCerebro.BlazorWasm.Services;

public class CardService : ApiService<CardDto, CreateCardCommand, UpdateCardCommand>, ICardService
{
    public CardService(HttpClient httpClient) : base(httpClient, "cards")
    {
    }

    public async Task<IEnumerable<CardDto>> GetByAccountIdAsync(Guid accountId)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<CardDto>>($"api/cards/account/{accountId}") ?? Enumerable.Empty<CardDto>();
    }
}