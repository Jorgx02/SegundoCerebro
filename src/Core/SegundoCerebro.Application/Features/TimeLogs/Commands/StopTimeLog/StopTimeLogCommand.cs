using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.TimeLogs.Commands.StopTimeLog;

public class StopTimeLogCommand : IRequest<TimeLogDto>
{
    public Guid TodoItemId { get; set; }
}