using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Data;

namespace SegundoCerebro.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio para la entidad Hábito (`Habit`).
/// </summary>
public class HabitRepository : Repository<Habit>, IHabitRepository
{
    public HabitRepository(ApplicationDbContext context) : base(context)
    {
    }
}