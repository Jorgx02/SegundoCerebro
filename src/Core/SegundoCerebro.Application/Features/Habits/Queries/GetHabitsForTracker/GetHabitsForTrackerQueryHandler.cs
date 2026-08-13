using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;

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
        // Obtener todos los hábitos del usuario, ordenados por DisplayOrder
        var habits = (await _unitOfWork.Habits.GetAllAsync()).ToList();

        // Obtener todos los logs de hábitos del usuario
        // Podríamos optimizar esto para solo cargar logs de un rango de fechas,
        // pero para el cálculo de rachas necesitamos el historial completo.
        var allHabitLogs = await _unitOfWork.HabitLogs.GetAllAsync();
        var logsByHabitId = allHabitLogs.GroupBy(log => log.HabitId)
                                        .ToDictionary(g => g.Key, g => g.OrderBy(l => l.Date).ToList());

        var habitDtos = new List<HabitDto>();

        foreach (var habit in habits)
        {
            var habitDto = _mapper.Map<HabitDto>(habit);
            habitDto.Logs = logsByHabitId.TryGetValue(habit.Id, out var logs)
                ? _mapper.Map<ICollection<HabitLogDto>>(logs)
                : new List<HabitLogDto>();

            // Calcular rachas
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
        if (!habitDto.Logs.Any())
        {
            habitDto.CurrentStreak = 0;
            habitDto.LongestStreak = 0;
            return;
        }

        var sortedLogs = habitDto.Logs.OrderBy(log => log.Date).ToList();
        var today = DateTime.Today;
        var yesterday = today.AddDays(-1);

        // Calcular racha actual
        int currentStreak = 0;
        DateTime? lastCompletedDate = null;

        // Buscar si el hábito se completó hoy o ayer
        if (sortedLogs.Any(log => log.Date.Date == today))
        {
            lastCompletedDate = today;
        }
        else if (sortedLogs.Any(log => log.Date.Date == yesterday))
        {
            lastCompletedDate = yesterday;
        }

        if (lastCompletedDate.HasValue)
        {
            currentStreak = 1;
            for (int i = sortedLogs.Count - 1; i >= 0; i--)
            {
                if (sortedLogs[i].Date.Date == lastCompletedDate.Value.AddDays(-currentStreak))
                {
                    currentStreak++;
                }
            }
        }
        habitDto.CurrentStreak = currentStreak;

        // Calcular racha más larga (LongestStreak)
        int longestStreak = 0;
        int tempStreak = 0;
        for (int i = 0; i < sortedLogs.Count; i++)
        {
            if (i == 0 || sortedLogs[i].Date.Date == sortedLogs[i - 1].Date.Date.AddDays(1))
            {
                tempStreak++;
            }
            else
            {
                tempStreak = 1;
            }
            if (tempStreak > longestStreak)
            {
                longestStreak = tempStreak;
            }
        }
        habitDto.LongestStreak = longestStreak;
    }
}