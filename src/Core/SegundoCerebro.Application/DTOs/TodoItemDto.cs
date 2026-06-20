using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Application.DTOs;

/// <summary>
/// DTO para representar los datos de una tarea (TodoItem) al ser consultada.
/// </summary>
public class TodoItemDto
{
    /// <summary>Identificador único de la tarea.</summary>
    public Guid Id { get; set; }
    /// <summary>Título de la tarea.</summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>Descripción detallada de la tarea.</summary>
    public string? Description { get; set; }
    /// <summary>Estado actual de la tarea según la metodología GTD (ej. Inbox, NextAction).</summary>
    public TodoItemStatus Status { get; set; }
    /// <summary>Nivel de prioridad de la tarea.</summary>
    public PriorityLevel Priority { get; set; }
    /// <summary>Fecha de vencimiento de la tarea.</summary>
    public DateTime? DueDate { get; set; }
    /// <summary>Fecha en que la tarea fue marcada como completada.</summary>
    public DateTime? CompletedAt { get; set; }
    /// <summary>Fecha de creación del registro.</summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>Fecha de la última actualización.</summary>
    public DateTime? UpdatedAt { get; set; }
    /// <summary>ID del proyecto al que pertenece la tarea.</summary>
    public Guid? ProjectId { get; set; }
    /// <summary>Nombre del proyecto asociado.</summary>
    public string? ProjectName { get; set; }
}