using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Habits.Commands.CreateHabit;
using SegundoCerebro.Application.Features.Habits.Commands.DeleteHabit;
using SegundoCerebro.Application.Features.Habits.Commands.UpdateHabit;
using SegundoCerebro.Application.Features.Habits.Commands.ToggleHabitCompletion;
using SegundoCerebro.Application.Features.Habits.Queries.GetHabitsForTracker;
using SegundoCerebro.Application.Features.Habits.Queries.GetAllHabits;

namespace SegundoCerebro.WebAPI.Controllers;

/// <summary>
/// Controlador para gestionar los hábitos de los usuarios.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HabitsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="HabitsController"/>.
    /// </summary>
    public HabitsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los hábitos del usuario actual.
    /// </summary>
    /// <returns>Una colección de hábitos.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HabitDto>>> GetAllHabits()
    {
        return Ok(await _mediator.Send(new GetAllHabitsQuery()));
    }

    /// <summary>
    /// Obtiene los hábitos con sus registros de cumplimiento para un rango de fechas.
    /// </summary>
    /// <param name="startDate">La fecha de inicio del rango.</param>
    /// <param name="endDate">La fecha de fin del rango.</param>
    /// <returns>Una colección de hábitos con sus logs.</returns>
    [HttpGet("tracker")]
    public async Task<ActionResult<IEnumerable<HabitDto>>> GetHabitsForTracker()
    {
        return Ok(await _mediator.Send(new GetHabitsForTrackerQuery()));
    }

    /// <summary>
    /// Obtiene un hábito específico por su ID. (Endpoint de marcador de posición)
    /// </summary>
    /// <param name="id">El ID del hábito a obtener.</param>
    [HttpGet("{id}")]
    public async Task<ActionResult<HabitDto>> GetHabit(Guid id)
    {
        await Task.CompletedTask;
        return NotFound("Endpoint no implementado todavía.");
    }

    /// <summary>
    /// Crea un nuevo hábito.
    /// </summary>
    /// <param name="habitDto">Los datos para el nuevo hábito.</param>
    /// <returns>El hábito recién creado.</returns>
    /// <response code="201">Devuelve el hábito recién creado y la URL para acceder a él.</response>
    /// <response code="400">Si los datos de entrada no son válidos.</responese>
    [HttpPost]
    public async Task<ActionResult<HabitDto>> CreateHabit([FromBody] CreateHabitDto habitDto)
    {
        var command = new CreateHabitCommand { HabitDto = habitDto };
        var habit = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetHabit), new { id = habit.Id }, habit);
    }

    /// <summary>
    /// Actualiza un hábito existente.
    /// </summary>
    /// <param name="id">El ID del hábito a actualizar.</param>
    /// <param name="habitDto">Los nuevos datos para el hábito.</param>
    /// <returns>El hábito actualizado.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<HabitDto>> UpdateHabit(Guid id, [FromBody] UpdateHabitDto habitDto)
    {
        var command = new UpdateHabitCommand { Id = id, HabitDto = habitDto };
        var habit = await _mediator.Send(command);
        return Ok(habit);
    }

    /// <summary>
    /// Elimina un hábito.
    /// </summary>
    /// <param name="id">El ID del hábito a eliminar.</param>
    /// <returns>No devuelve contenido.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteHabit(Guid id)
    {
        await _mediator.Send(new DeleteHabitCommand(id));
        return NoContent();
    }

    /// <summary>
    /// Registra o anula el cumplimiento de un hábito para una fecha específica.
    /// </summary>
    /// <param name="id">El ID del hábito.</param>
    /// <param name="command">El comando con la fecha del registro.</param>
    /// <returns>El nuevo estado de completado (true/false).</returns>
    [HttpPost("{id}/toggle")]
    public async Task<ActionResult<bool>> ToggleHabitCompletion(Guid id, [FromBody] ToggleHabitCompletionCommand command)
    {
        command.HabitId = id;
        var isCompleted = await _mediator.Send(command);
        return Ok(isCompleted);
    }
}