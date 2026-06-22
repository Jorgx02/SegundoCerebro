using Microsoft.EntityFrameworkCore;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Data;

namespace SegundoCerebro.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio para la entidad Tarea (`TodoItem`).
/// </summary>
public class TodoItemRepository : Repository<TodoItem>, ITodoItemRepository
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="TodoItemRepository"/>.
    /// </summary>
    /// <param name="context">El contexto de la base de datos.</param>
    public TodoItemRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtiene todas las tareas que pertenecen a un proyecto específico.
    /// </summary>
    public async Task<IEnumerable<TodoItem>> GetByProjectIdAsync(Guid projectId)
    {
        return await _dbSet
            .Where(t => t.ProjectId == projectId)
            .OrderBy(t => t.DueDate ?? DateTime.MaxValue) // Las que no tienen fecha van al final
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene todas las tareas que se encuentran en un estado GTD específico.
    /// </summary>
    public async Task<IEnumerable<TodoItem>> GetByStatusAsync(TodoItemStatus status)
    {
        return await _dbSet
            .Include(t => t.Project)
            .Where(t => t.Status == status)
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.DueDate ?? DateTime.MaxValue)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene todas las tareas que están en la "Bandeja de Entrada" (sin procesar).
    /// </summary>
    public async Task<IEnumerable<TodoItem>> GetInboxItemsAsync()
    {
        return await _dbSet
            .Where(t => t.Status == TodoItemStatus.Inbox)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene todas las tareas, incluyendo la información de su proyecto asociado (si lo tienen).
    /// </summary>
    public async Task<IEnumerable<TodoItem>> GetTodoItemsWithProjectsAsync()
    {
        return await _dbSet
            .Include(t => t.Project)
            .ToListAsync();
    }
}