using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Habits.Queries.GetHabitHeatmap;

/// <summary>
/// Consulta para obtener los datos del mapa de calor para todos los hábitos de un usuario en un año específico.
/// </summary>
/// <param name="Year">El año para el cual se solicitan los datos.</param>
public record GetHabitHeatmapQuery(int Year) : IRequest<IEnumerable<HabitHeatmapDto>>;

