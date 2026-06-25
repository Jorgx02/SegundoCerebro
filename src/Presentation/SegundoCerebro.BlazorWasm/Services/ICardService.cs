using SegundoCerebro.BlazorWasm.Models;
using SegundoCerebro.BlazorWasm.Models.Commands;

namespace SegundoCerebro.BlazorWasm.Services;

public interface ICardService
{
    Task<IEnumerable<CardDto>> GetByAccountIdAsync(Guid accountId);
    Task<CardDto> CreateAsync(CreateCardCommand command);
    Task DeleteAsync(Guid cardId);
}