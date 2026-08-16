using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SegundoCerebro.Application.Features.Habits.Queries.GetHabitStats;

/// <summary>
/// Manejador para la consulta GetHabitStatsQuery.
/// </summary>
public class GetHabitStatsQueryHandler : IRequestHandler<GetHabitStatsQuery, HabitStatsDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetHabitStatsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<HabitStatsDto> Handle(GetHabitStatsQuery request, CancellationToken cancellationToken)
    {
        var habits = (await _unitOfWork.Habits.GetAllAsync()).ToList();
        var logs = (await _unitOfWork.HabitLogs.GetAllAsync()).ToList();

        if (!habits.Any())
        {
            return new HabitStatsDto();
        }

        var stats = new HabitStatsDto
        {
            TotalHabits = habits.Count,
            TotalCompletions = logs.Count
        };

        var habitDetails = new List<(string Name, double Rate)>();
        double totalSuccessRate = 0;

        foreach (var habit in habits)
        {
            var daysSinceCreation = (DateTime.UtcNow - habit.CreatedAt).TotalDays;
            if (daysSinceCreation < 1) daysSinceCreation = 1;

            var habitLogs = logs.Where(l => l.HabitId == habit.Id).ToList();

            double expectedCompletions = (habit.Frequency == Domain.Enums.HabitFrequency.Daily)
                ? daysSinceCreation
                : daysSinceCreation / 7;

            if (expectedCompletions < 1) expectedCompletions = 1;

            var successRate = habitLogs.Any() ? (habitLogs.Count / expectedCompletions) * 100 : 0;
            habitDetails.Add((habit.Name, successRate));
            totalSuccessRate += successRate;
        }

        if (habitDetails.Any())
        {
            stats.OverallSuccessRate = totalSuccessRate / habitDetails.Count;
            var best = habitDetails.OrderByDescending(h => h.Rate).First();
            var worst = habitDetails.OrderBy(h => h.Rate).First();
            stats.BestHabitName = best.Name;
            stats.BestHabitSuccessRate = best.Rate;
            stats.WorstHabitName = worst.Name;
            stats.WorstHabitSuccessRate = worst.Rate;
        }

        // Consistencia Semanal
        var culture = new CultureInfo("es-ES");
        stats.CompletionsByDayOfWeek = Enum.GetValues(typeof(DayOfWeek))
            .Cast<DayOfWeek>()
            .ToDictionary(
                day => culture.DateTimeFormat.GetAbbreviatedDayName(day),
                day => logs.Count(l => l.Date.DayOfWeek == day)
            );

        // Progreso Mensual (últimos 12 meses)
        var twelveMonthsAgo = DateTime.UtcNow.AddMonths(-11);
        var firstDayOfStartMonth = new DateTime(twelveMonthsAgo.Year, twelveMonthsAgo.Month, 1);

        stats.CompletionsByMonth = logs
            .Where(l => l.Date >= firstDayOfStartMonth)
            .GroupBy(l => l.Date.ToString("yyyy-MM"))
            .ToDictionary(g => g.Key, g => g.Count());

        return stats;
    }
}