using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Application.DTOs;

/// <summary>
/// DTO para representar los datos de una tarea al ser consultada.
/// </summary>
public class TodoItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TodoItemStatus Status { get; set; }
    public string StatusName => Status.ToString();
    public int Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? ProjectId { get; set; }
    public string? ProjectName { get; set; }
    public TimeSpan TotalTimeTracked { get; set; }
    public bool IsCurrentlyTracking { get; set; }
}

/// <summary>
/// DTO utilizado para crear una nueva tarea.
/// </summary>
public class CreateTodoItemDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? ProjectId { get; set; }
}

/// <summary>
/// DTO utilizado para actualizar una tarea existente.
/// </summary>
public class UpdateTodoItemDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TodoItemStatus Status { get; set; }
    public int Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? ProjectId { get; set; }
}