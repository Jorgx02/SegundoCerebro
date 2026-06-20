using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Application.DTOs;

/// <summary>
/// DTO utilizado para crear una nueva tarea (TodoItem).
/// </summary>
public class CreateTodoItemDto
{
    /// <summary>Título o nombre de la tarea.</summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>Descripción detallada opcional de la tarea.</summary>
    public string? Description { get; set; }
    /// <summary>Nivel de prioridad de la tarea.</summary>
    public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
    /// <summary>Fecha de vencimiento opcional para la tarea.</summary>
    public DateTime? DueDate { get; set; }
    /// <summary>ID del proyecto al que pertenece la tarea. Si es nulo, va a la bandeja de entrada (Inbox).</summary>
    public Guid? ProjectId { get; set; }
}