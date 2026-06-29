using Microsoft.EntityFrameworkCore;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Data;

namespace SegundoCerebro.Infrastructure.Repositories;

public class TodoItemRepository : Repository<TodoItem>, ITodoItemRepository
{
    public TodoItemRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<TodoItem>> GetByProjectIdAsync(Guid projectId)
    {
        return await _dbSet
            .Where(t => t.ProjectId == projectId)
            .Include(t => t.Project)
            .Include(t => t.TimeLogs)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Sobrescribe el método base para incluir siempre el proyecto asociado (Eager Loading),
    /// permitiendo obtener el nombre del proyecto en las consultas de listado.
    /// </summary>
    public override async Task<IEnumerable<TodoItem>> GetAllAsync()
    {
        return await _dbSet
            .Include(t => t.Project) // Incluye la entidad Project
            .Include(t => t.TimeLogs)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }
}