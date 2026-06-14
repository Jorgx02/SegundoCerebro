using SegundoCerebro.BlazorWasm.Models.Enums;

namespace SegundoCerebro.BlazorWasm.Models;

public class CreateTodoItemDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
    public DateTime? DueDate { get; set; }
    public Guid? ProjectId { get; set; }
}