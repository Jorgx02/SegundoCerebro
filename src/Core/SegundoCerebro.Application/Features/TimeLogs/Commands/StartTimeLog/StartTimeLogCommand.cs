using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.TimeLogs.Commands.StartTimeLog;

public class StartTimeLogCommand : IRequest<TimeLogDto>
{
    public Guid TodoItemId { get; set; }
}