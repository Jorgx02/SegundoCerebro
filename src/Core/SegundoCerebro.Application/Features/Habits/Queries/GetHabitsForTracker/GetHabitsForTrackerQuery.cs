using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Habits.Queries.GetHabitsForTracker;

/// <summary>
/// Consulta para obtener los hábitos y sus registros de cumplimiento para un rango de fechas.
/// </summary>
public record GetHabitsForTrackerQuery() : IRequest<IEnumerable<HabitDto>>;