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

        // 4. Construir los DTOs para cada hábito, expandiendo fechas si es semanal.
        var heatmapData = allHabits.Select(habit =>
        {
            var dates = logsByHabit.TryGetValue(habit.Id, out var d) ? d : new HashSet<DateTime>();
            
            if (habit.Frequency == Domain.Enums.HabitFrequency.Weekly)
            {
                var expandedDates = new HashSet<DateTime>();
                foreach (var date in dates)
                {
                    int diff = (7 + (int)date.DayOfWeek - (int)DayOfWeek.Monday) % 7;
                    var monday = date.AddDays(-1 * diff).Date;
                    for (int i = 0; i < 7; i++)
                    {
                        expandedDates.Add(monday.AddDays(i));
                    }
                }
                dates = expandedDates;
            }

            return new HabitHeatmapDto
            {
                HabitId = habit.Id,
                HabitName = habit.Name,
                CompletedDates = dates
            };
        }).ToList();

        return heatmapData;
    }
}
