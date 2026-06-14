using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Application.DTOs;

public class UpdateTodoItemDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TodoItemStatus Status { get; set; }
    public PriorityLevel Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? ProjectId { get; set; }
}