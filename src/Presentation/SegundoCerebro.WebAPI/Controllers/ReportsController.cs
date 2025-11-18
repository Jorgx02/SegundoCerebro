using MediatR;
using Microsoft.AspNetCore.Mvc;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Reports.Queries.GetFinancialSummary;

namespace SegundoCerebro.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("financial-summary")]
    public async Task<ActionResult<FinancialSummaryDto>> GetFinancialSummary(
        [FromQuery] DateTime? startDate = null, 
        [FromQuery] DateTime? endDate = null)
    {
        var start = startDate ?? DateTime.Now.AddMonths(-12);
        var end = endDate ?? DateTime.Now;

        var summary = await _mediator.Send(new GetFinancialSummaryQuery(start, end));
        return Ok(summary);
    }
}