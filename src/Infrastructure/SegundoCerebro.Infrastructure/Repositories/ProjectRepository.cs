using Microsoft.EntityFrameworkCore;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Data;

namespace SegundoCerebro.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio para la entidad Proyecto (`Project`).
/// </summary>
public class ProjectRepository : Repository<Project>, IProjectRepository
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ProjectRepository"/>.
    /// </summary>
    /// <param name="context">El contexto de la base de datos.</param>
    public ProjectRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtiene todos los proyectos que no están completados ni cancelados.
    /// </summary>
    public async Task<IEnumerable<Project>> GetActiveProjectsAsync()
    {
        return await _dbSet
            .Where(p => p.Status != ProjectStatus.Completed && p.Status != ProjectStatus.Cancelled)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene un proyecto por su ID, incluyendo todas sus tareas (`TodoItem`) asociadas.
    /// </summary>
    public async Task<Project?> GetWithTodoItemsAsync(Guid id)
    {
        return await _dbSet
            .Include(p => p.TodoItems)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}