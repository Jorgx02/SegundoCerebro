namespace SegundoCerebro.Application.DTOs;

/// <summary>
/// DTO para representar una entrada del diario de bienestar al ser consultada.
/// </summary>
public class WellnessEntryDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public int MoodRating { get; set; }
    public int EnergyLevel { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// DTO para crear o actualizar una entrada del diario de bienestar.
/// </summary>
public class UpsertWellnessEntryDto
{
    public DateTime Date { get; set; }
    public int MoodRating { get; set; }
    public int EnergyLevel { get; set; }
    public string? Notes { get; set; }
}