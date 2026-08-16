using SegundoCerebro.BlazorWasm.Models;

namespace SegundoCerebro.BlazorWasm.Services;

/// <summary>
/// Define el contrato para el servicio de gestión de hábitos.
/// </summary>
public interface IHabitService : IApiService<HabitDto, CreateHabitDto, UpdateHabitDto>
{
    /// <summary>
    /// Obtiene los hábitos con sus registros para la vista del tracker.
    /// </summary>
    Task<IEnumerable<HabitDto>> GetHabitsForTrackerAsync();

    /// <summary>
    /// Registra o anula el cumplimiento de un hábito para una fecha específica.
    /// </summary>
    Task<bool> ToggleHabitCompletionAsync(Guid habitId, DateTime date);

    /// <summary>
    /// Obtiene los datos para el mapa de calor de un año específico.
    /// </summary>
    Task<IEnumerable<HabitHeatmapDto>> GetHeatmapDataAsync(int year);

    /// <summary>
    /// Obtiene las estadísticas agregadas de los hábitos.
    /// </summary>
    Task<HabitStatsDto> GetHabitStatsAsync();
}