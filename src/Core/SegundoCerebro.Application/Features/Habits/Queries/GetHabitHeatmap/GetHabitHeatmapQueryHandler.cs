using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Habits.Queries.GetHabitHeatmap;

/// <summary>
/// Manejador para la consulta GetHabitHeatmapQuery.
/// </summary>
public class GetHabitHeatmapQueryHandler : IRequestHandler<GetHabitHeatmapQuery, IEnumerable<HabitHeatmapDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetHabitHeatmapQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<HabitHeatmapDto>> Handle(GetHabitHeatmapQuery request, CancellationToken cancellationToken)
    {
        var year = request.Year;

        // 1. Obtener todos los hábitos del usuario (el filtro global se encarga del UserId).
        var allHabits = await _unitOfWork.Habits.GetAllAsync();

        // 2. Obtener todos los logs de hábitos del usuario para el año especificado.
        var allLogsForYear = await _unitOfWork.HabitLogs.GetLogsForYearAsync(year, cancellationToken);

        // 3. Agrupar los logs por hábito para una búsqueda eficiente.
        var logsByHabit = allLogsForYear
            .GroupBy(log => log.HabitId)
            .ToDictionary(g => g.Key, g => g.Select(l => l.Date).ToHashSet());

        // 4. Construir los DTOs para cada hábito, tenga o no tenga logs.
        var heatmapData = allHabits.Select(habit => new HabitHeatmapDto
        {
            HabitId = habit.Id,
            HabitName = habit.Name,
            CompletedDates = logsByHabit.TryGetValue(habit.Id, out var dates) ? dates : new HashSet<DateTime>()
        }).ToList();

        return heatmapData;
    }
}
