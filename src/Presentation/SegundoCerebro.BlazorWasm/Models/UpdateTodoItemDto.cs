using SegundoCerebro.BlazorWasm.Models.Enums;

namespace SegundoCerebro.BlazorWasm.Models;

public class UpdateTodoItemDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TodoItemStatus Status { get; set; }
    public PriorityLevel Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? ProjectId { get; set; }
}