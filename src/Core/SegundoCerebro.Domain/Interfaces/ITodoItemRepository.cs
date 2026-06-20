using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Domain.Interfaces;

/// <summary>
/// Define el contrato para el repositorio de Tareas (TodoItem), extendiendo el repositorio genérico.
/// Proporciona métodos de consulta específicos para la entidad TodoItem.
/// </summary>
public interface ITodoItemRepository : IRepository<TodoItem>
{
    /// <summary>
    /// Obtiene todas las tareas asociadas a un proyecto específico.
    /// </summary>
    /// <param name="projectId">El ID del proyecto.</param>
    /// <returns>Una colección de tareas del proyecto.</returns>
    Task<IEnumerable<TodoItem>> GetByProjectIdAsync(Guid projectId);
    /// <summary>
    /// Obtiene todas las tareas que se encuentran en un estado específico.
    /// </summary>
    /// <param name="status">El estado de la tarea a filtrar.</param>
    /// <returns>Una colección de tareas en el estado especificado.</returns>
    Task<IEnumerable<TodoItem>> GetByStatusAsync(TodoItemStatus status);
    /// <summary>
    /// Obtiene todas las tareas que están en la bandeja de entrada (Inbox), es decir, sin procesar.
    /// </summary>
    /// <returns>Una colección de tareas de la bandeja de entrada.</returns>
    Task<IEnumerable<TodoItem>> GetInboxItemsAsync();
    /// <summary>
    /// Obtiene todas las tareas del usuario, incluyendo la información de su proyecto asociado (Eager Loading).
    /// </summary>
    /// <returns>Una colección de tareas con sus proyectos.</returns>
    Task<IEnumerable<TodoItem>> GetTodoItemsWithProjectsAsync();
}