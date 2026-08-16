namespace SegundoCerebro.BlazorWasm.Models;

public class WellnessEntryDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public int MoodRating { get; set; }
    public int EnergyLevel { get; set; }
    public string? Notes { get; set; }
}

public class UpsertWellnessEntryDto
{
    public DateTime Date { get; set; }
    public int MoodRating { get; set; }
    public int EnergyLevel { get; set; }
    public string? Notes { get; set; }
}