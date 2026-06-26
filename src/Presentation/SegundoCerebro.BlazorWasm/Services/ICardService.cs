using SegundoCerebro.BlazorWasm.Models;
using SegundoCerebro.BlazorWasm.Models.Commands;

namespace SegundoCerebro.BlazorWasm.Services;

public interface ICardService : IApiService<CardDto, CreateCardCommand, UpdateCardCommand>
{
    Task<IEnumerable<CardDto>> GetByAccountIdAsync(Guid accountId);
}