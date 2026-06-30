using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Domain.Interfaces;

/// <summary>
/// Define el contrato para el repositorio de Hábitos (Habit).
/// </summary>
public interface IHabitRepository : IRepository<Habit>
{
    /// <summary>
    /// Obtiene los hábitos incluyendo todos sus registros de cumplimiento, ordenados por fecha.
    /// </summary>
    Task<IEnumerable<Habit>> GetHabitsWithAllLogsAsync();
}