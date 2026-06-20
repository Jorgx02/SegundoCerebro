using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Domain.Entities;

/// <summary>
/// Representa un elemento de tarea o recordatorio dentro del sistema de gestión de tareas.
/// </summary>
public class TodoItem
{
    /// <summary>Identificador del elemento de tarea.</summary>
    public Guid Id { get; set; }

    /// <summary>Nombre o título del elemento de tarea.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Descripción asociada al elemento de tarea.</summary>
    public string? Description { get; set; }

    /// <summary>Estado del elemento de tarea.</summary>
    public TodoItemStatus Status { get; set; } = TodoItemStatus.Inbox;

    /// <summary>Nivel de prioridad del elemento de tarea.</summary>
    public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;

    /// <summary>Fecha de vencimiento del elemento de tarea.</summary>
    public DateTime? DueDate { get; set; }

    /// <summary>Fecha de finalización del elemento de tarea.</summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>Fecha de creación del elemento de tarea.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Fecha de la última actualización del elemento de tarea.</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Id del usuario propietario del elemento de tarea.</summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>Id del proyecto al que pertenece el elemento de tarea.</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>Nombre identificativo del proyecto al que pertenece el elemento de tarea.</summary>
    public Project? Project { get; set; }
}