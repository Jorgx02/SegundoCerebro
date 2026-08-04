using Microsoft.EntityFrameworkCore;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Data;

namespace SegundoCerebro.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio para la entidad HabitLog.
/// </summary>
public class HabitLogRepository : Repository<HabitLog>, IHabitLogRepository
{
    public HabitLogRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<HabitLog>> GetLogsForYearAsync(int year, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(log => log.Date.Year == year).ToListAsync(cancellationToken);
    }

    public async Task<HabitLog?> GetLogForDateAsync(Guid habitId, DateTime date, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(log => log.HabitId == habitId && log.Date.Date == date.Date, cancellationToken);
    }
}