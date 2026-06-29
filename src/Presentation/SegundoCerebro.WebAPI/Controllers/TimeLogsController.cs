using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.TimeLogs.Commands.StartTimeLog;
using SegundoCerebro.Application.Features.TimeLogs.Commands.StopTimeLog;

namespace SegundoCerebro.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TimeLogsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TimeLogsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("start")]
    public async Task<ActionResult<TimeLogDto>> Start([FromBody] StartTimeLogCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPost("stop")]
    public async Task<ActionResult<TimeLogDto>> Stop([FromBody] StopTimeLogCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}