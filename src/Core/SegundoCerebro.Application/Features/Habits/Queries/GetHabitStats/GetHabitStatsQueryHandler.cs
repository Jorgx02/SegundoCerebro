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
        var dailyLogsCount = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToDictionary(d => d, d => 0);
        foreach (var habit in habits)
        {
            var hLogs = logs.Where(l => l.HabitId == habit.Id).ToList();
            if (habit.Frequency == Domain.Enums.HabitFrequency.Daily)
            {
                foreach (var log in hLogs)
                {
                    dailyLogsCount[log.Date.DayOfWeek]++;
                }
            }
            else
            {
                foreach (var log in hLogs)
                {
                    foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                    {
                        dailyLogsCount[day]++;
                    }
                }
            }
        }

        stats.CompletionsByDayOfWeek = Enum.GetValues(typeof(DayOfWeek))
            .Cast<DayOfWeek>()
            .ToDictionary(
                day => day.ToString(),
                day => dailyLogsCount[day]
            );

        // Progreso Mensual (Días de este mes) — los hábitos semanales suman a todos los días de su semana
        var today = DateTime.UtcNow;
        int daysInThisMonth = DateTime.DaysInMonth(today.Year, today.Month);

        for (int i = 1; i <= daysInThisMonth; i++)
            stats.CompletionsThisMonth[i] = 0;

        foreach (var habit in habits)
        {
            var hLogs = logs.Where(l => l.HabitId == habit.Id && l.Date.Year == today.Year && l.Date.Month == today.Month).ToList();
            if (habit.Frequency == Domain.Enums.HabitFrequency.Daily)
            {
                foreach (var log in hLogs)
                    if (stats.CompletionsThisMonth.ContainsKey(log.Date.Day))
                        stats.CompletionsThisMonth[log.Date.Day]++;
            }
            else // Weekly: expand log to all 7 days of its week, clipped to this month
            {
                foreach (var log in hLogs)
                {
                    int diff = (7 + (int)log.Date.DayOfWeek - (int)DayOfWeek.Monday) % 7;
                    var monday = log.Date.AddDays(-diff).Date;
                    for (int d = 0; d < 7; d++)
                    {
                        var dayOfWeek = monday.AddDays(d);
                        if (dayOfWeek.Year == today.Year && dayOfWeek.Month == today.Month
                            && stats.CompletionsThisMonth.ContainsKey(dayOfWeek.Day))
                            stats.CompletionsThisMonth[dayOfWeek.Day]++;
                    }
                }
            }
        }

        // Progreso Anual (enero → diciembre del año en curso)
        var firstDayOfYear = new DateTime(today.Year, 1, 1);

        // Initialize all 12 months of current year with 0
        for (int m = 1; m <= 12; m++)
        {
            var key = new DateTime(today.Year, m, 1).ToString("yyyy-MM");
            stats.CompletionsByMonth[key] = 0;
        }

        foreach (var habit in habits)
        {
            var hLogs = logs.Where(l => l.HabitId == habit.Id && l.Date >= firstDayOfYear && l.Date.Year == today.Year).ToList();
            foreach (var log in hLogs)
            {
                var key = log.Date.ToString("yyyy-MM");
                if (stats.CompletionsByMonth.ContainsKey(key))
                    stats.CompletionsByMonth[key]++;
            }
        }

        return stats;
    }
}