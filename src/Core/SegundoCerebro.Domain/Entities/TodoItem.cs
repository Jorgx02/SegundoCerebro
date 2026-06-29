using SegundoCerebro.Domain.Enums;
using System.Collections.Generic;

namespace SegundoCerebro.Domain.Entities;

/// <summary>
/// Representa una tarea o acción individual dentro del sistema de productividad.
/// </summary>
public class TodoItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TodoItemStatus Status { get; set; }
    public int Priority { get; set; } // 0 = None, 1 = Low, 2 = Medium, 3 = High
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string UserId { get; set; } = string.Empty;

    // Foreign Key
    public Guid? ProjectId { get; set; }
    // Navigation Property
    public Project? Project { get; set; }

    public ICollection<TimeLog> TimeLogs { get; set; } = new List<TimeLog>();
}