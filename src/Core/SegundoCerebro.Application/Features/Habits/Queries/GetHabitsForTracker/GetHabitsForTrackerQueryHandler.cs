using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using System.Collections.Generic;

namespace SegundoCerebro.Application.Features.Habits.Queries.GetHabitsForTracker;

/// <summary>
/// Manejador para la consulta que obtiene los datos para el tracker de hábitos.
/// </summary>
public class GetHabitsForTrackerQueryHandler : IRequestHandler<GetHabitsForTrackerQuery, IEnumerable<HabitDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetHabitsForTrackerQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<HabitDto>> Handle(GetHabitsForTrackerQuery request, CancellationToken cancellationToken)
    {
        var habits = await _unitOfWork.Habits.GetHabitsWithAllLogsAsync();
        var habitDtos = new List<HabitDto>();

        foreach (var habit in habits)
        {
            var dto = _mapper.Map<HabitDto>(habit);
            var (currentStreak, longestStreak) = CalculateStreaks(habit.Logs);
            dto.CurrentStreak = currentStreak;
            dto.LongestStreak = longestStreak;
            habitDtos.Add(dto);
        }

        return habitDtos;
    }

    /// <summary>
    /// Calcula la racha actual y la más larga para un hábito basándose en sus registros.
    /// </summary>
    /// <param name="logs">La colección de registros de cumplimiento del hábito.</param>
    /// <returns>Una tupla con la racha actual y la racha más larga.</returns>
    private (int currentStreak, int longestStreak) CalculateStreaks(ICollection<HabitLog> logs)
    {
        if (logs == null || !logs.Any()) return (0, 0);

        var dates = logs.Select(l => l.Date.Date).ToHashSet();
        var today = DateTime.Today;

        // --- Current Streak Calculation ---
        int currentStreak = 0;
        DateTime dateToCheck;

        if (dates.Contains(today))
        {
            dateToCheck = today;
        }
        else if (dates.Contains(today.AddDays(-1)))
        {
            dateToCheck = today.AddDays(-1);
        }
        else
        {
            // Streak is broken if not completed today or yesterday
            return (0, CalculateLongestStreak(dates));
        }

        currentStreak = 1;
        var previousDay = dateToCheck.AddDays(-1);
        while (dates.Contains(previousDay))
        {
            currentStreak++;
            previousDay = previousDay.AddDays(-1);
        }

        return (currentStreak, CalculateLongestStreak(dates));
    }

    private int CalculateLongestStreak(HashSet<DateTime> dates)
    {
        if (!dates.Any()) return 0;

        var orderedDates = dates.OrderBy(d => d).ToList();
        int longestStreak = 0;
        int tempStreak = 0;
        if (orderedDates.Count > 0) {
            longestStreak = 1;
            tempStreak = 1;
        }
        
        for (int i = 1; i < orderedDates.Count; i++)
        {
            if (orderedDates[i].Date == orderedDates[i - 1].Date.AddDays(1))
            {
                tempStreak++;
            }
            else
            {
                longestStreak = Math.Max(longestStreak, tempStreak);
                tempStreak = 1;
            }
        }
        longestStreak = Math.Max(longestStreak, tempStreak);

        return longestStreak;
    }
}