using SegundoCerebro.BlazorWasm.Models;

namespace SegundoCerebro.BlazorWasm.Services;

public interface ITimeLogService
{
    Task<TimeLogDto> StartAsync(Guid todoItemId);
    Task<TimeLogDto> StopAsync(Guid todoItemId);
}