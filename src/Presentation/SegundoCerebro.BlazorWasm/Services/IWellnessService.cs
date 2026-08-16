using SegundoCerebro.BlazorWasm.Models;

namespace SegundoCerebro.BlazorWasm.Services;

public interface IWellnessService
{
    Task<IEnumerable<WellnessEntryDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<WellnessEntryDto> UpsertAsync(UpsertWellnessEntryDto dto);
}