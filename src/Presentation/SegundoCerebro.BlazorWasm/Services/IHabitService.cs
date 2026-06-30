using SegundoCerebro.BlazorWasm.Models;

namespace SegundoCerebro.BlazorWasm.Services;

/// <summary>
/// Define el contrato para el servicio que interactúa con la API de Hábitos.
/// </summary>
public interface IHabitService : IApiService<HabitDto, CreateHabitDto, UpdateHabitDto>
{
    /// <summary>
    /// Obtiene los datos de los hábitos para el tracker en un rango de fechas.
    /// </summary>
    /// <param name="startDate">Fecha de inicio.</param>
    /// <param name="endDate">Fecha de fin.</param>
    /// <returns>Una colección de hábitos con sus registros de cumplimiento.</returns>
    Task<IEnumerable<HabitDto>> GetHabitsForTrackerAsync();

    /// <summary>
    /// Registra o anula el cumplimiento de un hábito para una fecha.
    /// </summary>
    /// <param name="habitId">ID del hábito.</param>
    /// <param name="date">Fecha del registro.</param>
    /// <returns>El nuevo estado de completado.</returns>
    Task<bool> ToggleHabitCompletionAsync(Guid habitId, DateTime date);
}