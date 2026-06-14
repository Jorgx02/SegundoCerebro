using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Domain.Entities;

public class TodoItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TodoItemStatus Status { get; set; } = TodoItemStatus.Inbox;
    public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relación con el usuario propietario (Aislamiento de datos)
    public string UserId { get; set; } = string.Empty;

    public Guid? ProjectId { get; set; }
    public Project? Project { get; set; }
}