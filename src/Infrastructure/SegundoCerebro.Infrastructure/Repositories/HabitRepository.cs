using Microsoft.EntityFrameworkCore;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Data;

namespace SegundoCerebro.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio para la entidad Habit.
/// </summary>
public class HabitRepository : Repository<Habit>, IHabitRepository
{
    public HabitRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Habit>> GetHabitsWithAllLogsAsync()
    {
        return await _dbSet
            .Include(h => h.Logs.OrderByDescending(l => l.Date))
            .OrderBy(h => h.CreatedAt)
            .ToListAsync();
    }
}