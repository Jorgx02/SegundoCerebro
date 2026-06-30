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

    /// <inheritdoc />
    public async Task<HabitLog?> GetLogForDateAsync(Guid habitId, DateTime date)
    {
        return await _dbSet.FirstOrDefaultAsync(hl => hl.HabitId == habitId && hl.Date == date.Date);
    }
}