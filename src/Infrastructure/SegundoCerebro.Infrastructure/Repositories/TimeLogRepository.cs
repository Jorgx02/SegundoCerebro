using Microsoft.EntityFrameworkCore;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Data;

namespace SegundoCerebro.Infrastructure.Repositories;

public class TimeLogRepository : Repository<TimeLog>, ITimeLogRepository
{
    public TimeLogRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<TimeLog?> GetActiveLogForTaskAsync(Guid todoItemId)
    {
        return await _dbSet.FirstOrDefaultAsync(tl => tl.TodoItemId == todoItemId && tl.EndTime == null);
    }
}