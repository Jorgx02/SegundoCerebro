using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Application.DTOs;

/// <summary>
/// DTO utilizado para actualizar una tarea (TodoItem) existente.
/// </summary>
public class UpdateTodoItemDto
{
    /// <summary>Nuevo título de la tarea.</summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>Nueva descripción opcional.</summary>
    public string? Description { get; set; }
    /// <summary>Nuevo estado de la tarea.</summary>
    public TodoItemStatus Status { get; set; }
    /// <summary>Nuevo nivel de prioridad.</summary>
    public PriorityLevel Priority { get; set; }
    /// <summary>Nueva fecha de vencimiento.</summary>
    public DateTime? DueDate { get; set; }
    /// <summary>Nuevo ID del proyecto asociado.</summary>
    public Guid? ProjectId { get; set; }
}