using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Wellness.Commands.UpsertWellnessEntry;
using SegundoCerebro.Application.Features.Wellness.Queries.GetWellnessEntriesByDateRange;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SegundoCerebro.WebAPI.Controllers;

/// <summary>
/// Controlador para gestionar el diario de bienestar del usuario.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WellnessController : ControllerBase
{
    private readonly IMediator _mediator;

    public WellnessController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene las entradas del diario de bienestar para un rango de fechas específico.
    /// </summary>
    /// <param name="startDate">La fecha de inicio del rango.</param>
    /// <param name="endDate">La fecha de fin del rango.</param>
    /// <returns>Una colección de entradas de bienestar.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WellnessEntryDto>>> GetWellnessEntries([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var query = new GetWellnessEntriesByDateRangeQuery(startDate, endDate);
        var entries = await _mediator.Send(query);
        return Ok(entries);
    }

    /// <summary>
    /// Crea o actualiza una entrada en el diario de bienestar para una fecha específica.
    /// Si ya existe una entrada para la fecha proporcionada, se actualiza. Si no, se crea una nueva.
    /// </summary>
    /// <param name="dto">Los datos para la entrada del diario.</param>
    /// <returns>La entrada del diario creada o actualizada.</returns>
    [HttpPost]
    public async Task<ActionResult<WellnessEntryDto>> UpsertWellnessEntry([FromBody] UpsertWellnessEntryDto dto)
    {
        var command = new UpsertWellnessEntryCommand(dto);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}