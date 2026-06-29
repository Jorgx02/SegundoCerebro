namespace SegundoCerebro.Domain.Entities;

/// <summary>
/// Representa un registro de tiempo dedicado a una tarea específica.
/// </summary>
public class TimeLog
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public TimeSpan Duration => EndTime.HasValue ? EndTime.Value - StartTime : TimeSpan.Zero;
    public string? Notes { get; set; }
    public Guid TodoItemId { get; set; }
    public TodoItem TodoItem { get; set; } = null!;
    public string UserId { get; set; } = string.Empty;
}