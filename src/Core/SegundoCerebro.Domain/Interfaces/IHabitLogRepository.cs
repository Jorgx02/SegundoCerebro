using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Domain.Interfaces;

/// <summary>
/// Define el contrato para el repositorio de Registros de Hábitos (HabitLog).
/// </summary>
public interface IHabitLogRepository : IRepository<HabitLog>
{
    /// <summary>
    /// Obtiene todos los registros de un año específico para el usuario actual.
    /// </summary>
    Task<IEnumerable<HabitLog>> GetLogsForYearAsync(int year, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un registro de hábito para un hábito y fecha específicos.
    /// </summary>
    /// <param name="habitId">El ID del hábito.</param>
    /// <param name="date">La fecha del registro.</param>
    /// <returns>El registro de hábito encontrado, o null si no existe.</returns>
    Task<HabitLog?> GetLogForDateAsync(Guid habitId, DateTime date, CancellationToken cancellationToken = default);
}