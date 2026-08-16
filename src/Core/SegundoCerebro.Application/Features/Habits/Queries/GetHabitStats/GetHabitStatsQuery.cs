using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Habits.Queries.GetHabitStats;

/// <summary>
/// Consulta para obtener las estadísticas agregadas de los hábitos de un usuario.
/// </summary>
public record GetHabitStatsQuery : IRequest<HabitStatsDto>;