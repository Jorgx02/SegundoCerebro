using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Domain.Interfaces;

/// <summary>
/// Define el contrato para el repositorio de Hábitos (Habit).
/// </summary>
public interface IHabitRepository : IRepository<Habit>
{
}