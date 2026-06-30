using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Domain.Interfaces;

/// <summary>
/// Define el contrato para el repositorio de Registros de Hábitos (HabitLog).
/// </summary>
public interface IHabitLogRepository : IRepository<HabitLog>
{
    /// <summary>
    /// Obtiene el registro de un hábito para una fecha específica.
    /// </summary>
    /// <param name="habitId">El ID del hábito.</param>
    /// <param name="date">La fecha del registro.</param>
    /// <returns>El registro de hábito si existe, o null.</returns>
    Task<HabitLog?> GetLogForDateAsync(Guid habitId, DateTime date);
}