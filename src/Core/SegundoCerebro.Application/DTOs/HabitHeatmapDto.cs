namespace SegundoCerebro.Application.DTOs;

/// <summary>
/// DTO para representar los datos necesarios para un mapa de calor de un hábito.
/// </summary>
public class HabitHeatmapDto
{
    /// <summary>
    /// Identificador del hábito.
    /// </summary>
    public Guid HabitId { get; set; }

    /// <summary>
    /// Nombre del hábito.
    /// </summary>
    public string HabitName { get; set; } = string.Empty;

    /// <summary>
    /// Conjunto de fechas en las que el hábito fue completado.
    /// </summary>
    public HashSet<DateTime> CompletedDates { get; set; } = new();
}