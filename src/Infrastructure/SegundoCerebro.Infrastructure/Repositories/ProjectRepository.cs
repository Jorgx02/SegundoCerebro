using Microsoft.EntityFrameworkCore;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Data;

namespace SegundoCerebro.Infrastructure.Repositories;

public class ProjectRepository : Repository<Project>, IProjectRepository
{
    public ProjectRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Project?> GetWithTodoItemsAsync(Guid projectId)
    {
        return await _dbSet
            .Include(p => p.TodoItems)
            .FirstOrDefaultAsync(p => p.Id == projectId);
    }
}