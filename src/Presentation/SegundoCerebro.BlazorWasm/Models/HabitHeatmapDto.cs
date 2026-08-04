namespace SegundoCerebro.BlazorWasm.Models;

public class HabitHeatmapDto
{
    public Guid HabitId { get; set; }
    public string HabitName { get; set; } = string.Empty;
    public HashSet<DateTime> CompletedDates { get; set; } = new();
}

