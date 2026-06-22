using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Domain.Entities;

/// <summary>
/// Representa una tarea individual o un elemento de acción, siguiendo principios de GTD (Getting Things Done).
/// </summary>
public class TodoItem
{
    /// <summary>Identificador único de la tarea.</summary>
    public Guid Id { get; set; }

    /// <summary>Título conciso que describe la acción a realizar.</summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>Descripción detallada opcional con más contexto sobre la tarea.</summary>
    public string? Description { get; set; }

    /// <summary>Estado de la tarea dentro del flujo GTD (ej. Inbox, NextAction, Completed).</summary>
    public TodoItemStatus Status { get; set; } = TodoItemStatus.Inbox;

    /// <summary>Nivel de urgencia o importancia de la tarea.</summary>
    public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;

    /// <summary>Fecha límite opcional para completar la tarea.</summary>
    public DateTime? DueDate { get; set; }

    /// <summary>Fecha y hora en que la tarea fue marcada como completada.</summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>Fecha de creación del registro.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Fecha de la última actualización del registro.</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Identificador del usuario propietario de la tarea (Multi-tenancy).</summary>
    public string UserId { get; set; } = string.Empty;

    // Relaciones
    /// <summary>Clave foránea opcional al proyecto al que pertenece. Si es nulo, la tarea está en el "Inbox".</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>Propiedad de navegación al proyecto asociado.</summary>
    public Project? Project { get; set; }
}