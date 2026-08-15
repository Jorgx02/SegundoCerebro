using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace SegundoCerebro.Application.Features.Habits.Queries.GetHabitsForTracker;

/// <summary>
/// Manejador para la consulta GetHabitsForTrackerQuery.
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

    /// <summary>
    /// Procesa la solicitud para obtener los hábitos del usuario con sus logs para el tracker.
    /// </summary>
    /// <param name="request">La consulta.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Una colección de DTOs de hábitos con sus logs y rachas calculadas.</returns>
    public async Task<IEnumerable<HabitDto>> Handle(GetHabitsForTrackerQuery request, CancellationToken cancellationToken)
    {
        // 1. Obtener todos los hábitos del usuario.
        var habits = (await _unitOfWork.Habits.GetAllAsync()).ToList();

        // 2. Obtener todos los logs de hábitos del usuario.
        // Se necesita el historial completo para calcular correctamente las rachas.
        var allHabitLogs = await _unitOfWork.HabitLogs.GetAllAsync();

        // 3. Agrupar los logs por el ID del hábito para una búsqueda eficiente.
        var logsByHabitId = allHabitLogs.GroupBy(log => log.HabitId)
                                        .ToDictionary(g => g.Key, g => g.ToList());

        var habitDtos = new List<HabitDto>();

        foreach (var habit in habits)
        {
            var habitDto = _mapper.Map<HabitDto>(habit);

            // 4. Asignar los logs correspondientes a cada hábito.
            habitDto.Logs = logsByHabitId.TryGetValue(habit.Id, out var logs)
                ? _mapper.Map<ICollection<HabitLogDto>>(logs)
                : new List<HabitLogDto>();

            // 5. Calcular las rachas para el hábito.
            CalculateStreaks(habitDto);
            habitDtos.Add(habitDto);
        }

        return habitDtos;
    }

    /// <summary>
    /// Calcula la racha actual y la racha más larga para un hábito.
    /// </summary>
    /// <param name="habitDto">El DTO del hábito con sus logs.</param>
    private void CalculateStreaks(HabitDto habitDto)
    {
        if (habitDto.Logs == null || !habitDto.Logs.Any())
        {
            habitDto.CurrentStreak = 0;
            habitDto.LongestStreak = 0;
            return;
        }

        if (habitDto.Frequency == Domain.Enums.HabitFrequency.Daily)
        {
            CalculateDailyStreaks(habitDto);
        }
        else if (habitDto.Frequency == Domain.Enums.HabitFrequency.Weekly)
        {
            CalculateWeeklyStreaks(habitDto);
        }
    }

    private void CalculateDailyStreaks(HabitDto habitDto)
    {
        var logDates = habitDto.Logs.Select(l => l.Date.Date).ToHashSet();
        var today = DateTime.UtcNow.Date;

        // --- Calcular Racha Actual ---
        int currentStreak = 0;
        if (logDates.Contains(today) || logDates.Contains(today.AddDays(-1)))
        {
            var dateToCheck = logDates.Contains(today) ? today : today.AddDays(-1);
            while (logDates.Contains(dateToCheck))
            {
                currentStreak++;
                dateToCheck = dateToCheck.AddDays(-1);
            }
        }
        habitDto.CurrentStreak = currentStreak;

        // --- Calcular Racha Más Larga ---
        var sortedDates = logDates.OrderBy(d => d).ToList();
        int longestStreak = 0;
        int tempStreak = 0;
        if (sortedDates.Any())
        {
            tempStreak = 1;
            longestStreak = 1;
            for (int i = 1; i < sortedDates.Count; i++)
            {
                if (sortedDates[i] == sortedDates[i - 1].AddDays(1))
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
        }
        habitDto.LongestStreak = longestStreak;
    }

    private DateTime GetStartOfWeek(DateTime date)
    {
        int diff = (7 + (int)date.DayOfWeek - (int)DayOfWeek.Monday) % 7;
        return date.AddDays(-1 * diff).Date;
    }

    private void CalculateWeeklyStreaks(HabitDto habitDto)
    {
        var logDates = habitDto.Logs.Select(l => l.Date.Date).ToHashSet();
        var today = DateTime.UtcNow.Date;
        var startOfThisWeek = GetStartOfWeek(today);
        var startOfLastWeek = startOfThisWeek.AddDays(-7);

        // --- Calculate Current Streak ---
        int currentStreak = 0;
        bool completedThisWeek = logDates.Any(d => d >= startOfThisWeek);
        bool completedLastWeek = logDates.Any(d => d >= startOfLastWeek && d < startOfThisWeek);

        if (completedThisWeek || completedLastWeek)
        {
            var weekToCheckStart = completedThisWeek ? startOfThisWeek : startOfLastWeek;
            while (logDates.Any(d => d >= weekToCheckStart && d < weekToCheckStart.AddDays(7)))
            {
                currentStreak++;
                weekToCheckStart = weekToCheckStart.AddDays(-7);
            }
        }
        habitDto.CurrentStreak = currentStreak;

        // --- Calculate Longest Streak ---
        var weekStarts = logDates.Select(GetStartOfWeek).Distinct().OrderBy(d => d).ToList();
        int longestStreak = 0;
        int tempStreak = 0;
        if (weekStarts.Any())
        {
            tempStreak = 1;
            longestStreak = 1;
            for (int i = 1; i < weekStarts.Count; i++)
            {
                if (weekStarts[i] == weekStarts[i - 1].AddDays(7))
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
        }
        habitDto.LongestStreak = longestStreak;
    }
}
