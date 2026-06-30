using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Habits.Queries.GetAllHabits;

/// <summary>
/// Comando para obtener todos los hábitos del usuario actual.
/// </summary>
public record GetAllHabitsQuery : IRequest<IEnumerable<HabitDto>>;