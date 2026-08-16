namespace SegundoCerebro.BlazorWasm.Models;

public class HabitStatsDto
{
    public double OverallSuccessRate { get; set; }
    public int TotalHabits { get; set; }
    public int TotalCompletions { get; set; }
    public string? BestHabitName { get; set; }
    public double BestHabitSuccessRate { get; set; }
    public string? WorstHabitName { get; set; }
    public double WorstHabitSuccessRate { get; set; }
    public Dictionary<string, int> CompletionsByDayOfWeek { get; set; } = new();
    public Dictionary<string, int> CompletionsByMonth { get; set; } = new();
}