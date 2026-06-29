namespace SegundoCerebro.Application.DTOs;

public class TimeLogDto
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public TimeSpan Duration { get; set; }
    public string? Notes { get; set; }
    public Guid TodoItemId { get; set; }
}