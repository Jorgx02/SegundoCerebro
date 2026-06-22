using MediatR;
using Microsoft.AspNetCore.Mvc;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Budgets.Commands.CreateBudget;
using SegundoCerebro.Application.Features.Budgets.Commands.UpdateBudget;
using SegundoCerebro.Application.Features.Budgets.Commands.DeleteBudget;
using SegundoCerebro.Application.Features.Budgets.Queries.GetAllBudgets;
using SegundoCerebro.Application.Features.Budgets.Queries.GetBudgetById;

namespace SegundoCerebro.WebAPI.Controllers;

/// <summary>
/// Controlador para la gestión de presupuestos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BudgetsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BudgetsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los presupuestos activos del usuario.
    /// </summary>
    /// <returns>Una colección de presupuestos.</returns>
    /// <response code="200">Devuelve la lista de presupuestos.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BudgetDto>>> GetAllBudgets()
    {
        var budgets = await _mediator.Send(new GetAllBudgetsQuery());
        return Ok(budgets);
    }

    /// <summary>
    /// Obtiene un presupuesto específico por su ID.
    /// </summary>
    /// <param name="id">El ID del presupuesto a obtener.</param>
    /// <returns>El presupuesto solicitado.</returns>
    /// <response code="200">Devuelve el presupuesto encontrado.</response>
    /// <response code="404">Si no se encuentra un presupuesto con el ID especificado.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<BudgetDto>> GetBudget(Guid id)
    {
        var budget = await _mediator.Send(new GetBudgetByIdQuery(id));

        if (budget == null)
            return NotFound();

        return Ok(budget);
    }

    /// <summary>
    /// Crea un nuevo presupuesto.
    /// </summary>
    /// <param name="createBudgetDto">Los datos para el nuevo presupuesto.</param>
    /// <returns>El presupuesto recién creado.</returns>
    /// <response code="201">Devuelve el presupuesto recién creado.</response>
    /// <response code="400">Si los datos de entrada no son válidos.</response>
    [HttpPost]
    public async Task<ActionResult<BudgetDto>> CreateBudget(CreateBudgetDto createBudgetDto)
    {
        var budget = await _mediator.Send(new CreateBudgetCommand(createBudgetDto));
        return CreatedAtAction(nameof(GetBudget), new { id = budget.Id }, budget);
    }

    /// <summary>
    /// Actualiza un presupuesto existente.
    /// </summary>
    /// <param name="id">El ID del presupuesto a actualizar.</param>
    /// <param name="updateBudgetDto">Los nuevos datos para el presupuesto.</param>
    /// <returns>El presupuesto actualizado.</returns>
    /// <response code="200">Devuelve el presupuesto actualizado.</response>
    /// <response code="404">Si no se encuentra un presupuesto con el ID especificado.</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<BudgetDto>> UpdateBudget(Guid id, UpdateBudgetDto updateBudgetDto)
    {
        try
        {
            var budget = await _mediator.Send(new UpdateBudgetCommand(id, updateBudgetDto));
            return Ok(budget);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Elimina un presupuesto existente.
    /// </summary>
    /// <param name="id">El ID del presupuesto a eliminar.</param>
    /// <returns>No devuelve contenido.</returns>
    /// <response code="204">Si el presupuesto se eliminó correctamente.</response>
    /// <response code="404">Si no se encuentra un presupuesto con el ID especificado.</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBudget(Guid id)
    {
        var result = await _mediator.Send(new DeleteBudgetCommand(id));

        if (!result)
            return NotFound();

        return NoContent();
    }
}