// filepath: src/Presentation/SegundoCerebro.WebAPI/Controllers/ReportsController.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Reports.Queries.GetFinancialSummary;
using SegundoCerebro.Application.Features.Reports.Queries.ExportTransactionsToExcel;
using SegundoCerebro.Application.Features.Reports.Queries.ExportTransactionsToPdf;

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

    [HttpGet("export/excel")]
    public async Task<IActionResult> ExportToExcel(
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate,
        [FromQuery] Guid? accountId = null)
    {
        var fileBytes = await _mediator.Send(new ExportTransactionsToExcelQuery(startDate, endDate, accountId));
        
        return File(
            fileBytes, 
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
            $"transactions_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.xlsx");
    }

    [HttpGet("export/pdf")]
    public async Task<IActionResult> ExportToPdf(
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate,
        [FromQuery] Guid? accountId = null)
    {
        var fileBytes = await _mediator.Send(new ExportTransactionsToPdfQuery(startDate, endDate, accountId));
        
        return File(
            fileBytes, 
            "application/pdf", 
            $"transactions_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.pdf");
    }

    [HttpGet("export/transaction/{id}/pdf")]
    public async Task<IActionResult> ExportTransactionToPdf(Guid id)
    {
        var fileBytes = await _mediator.Send(new SegundoCerebro.Application.Features.Reports.Queries.ExportSingleTransactionToPdf.ExportSingleTransactionToPdfQuery(id));
        return File(fileBytes, "application/pdf", $"factura_{id.ToString().Substring(0, 8)}.pdf");
    }
}