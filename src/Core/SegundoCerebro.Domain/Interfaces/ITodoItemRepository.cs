using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Domain.Interfaces;

/// <summary>
/// Define el contrato para el repositorio de Tareas (TodoItem).
/// </summary>
public interface ITodoItemRepository : IRepository<TodoItem>
{
    Task<IEnumerable<TodoItem>> GetByProjectIdAsync(Guid projectId);
}