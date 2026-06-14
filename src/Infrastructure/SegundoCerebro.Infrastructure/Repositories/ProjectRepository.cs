using Microsoft.EntityFrameworkCore;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Data;

namespace SegundoCerebro.Infrastructure.Repositories;

public class ProjectRepository : Repository<Project>, IProjectRepository
{
    public ProjectRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Project>> GetActiveProjectsAsync()
    {
        return await _dbSet
            .Where(p => p.Status != ProjectStatus.Completed && p.Status != ProjectStatus.Cancelled)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Project?> GetWithTodoItemsAsync(Guid id)
    {
        return await _dbSet
            .Include(p => p.TodoItems)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}