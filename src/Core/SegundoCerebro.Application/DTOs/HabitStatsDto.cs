namespace SegundoCerebro.Application.DTOs;

/// <summary>
/// DTO para transportar las estadísticas agregadas de los hábitos.
/// </summary>
public class HabitStatsDto
{
    public double OverallSuccessRate { get; set; }
    public int TotalHabits { get; set; }
    public int TotalCompletions { get; set; }
    public string? BestHabitName { get; set; }
    public double BestHabitSuccessRate { get; set; }
    public string? WorstHabitName { get; set; }
    public double WorstHabitSuccessRate { get; set; }
    /// <summary>
    /// Clave: Nombre abreviado del día ("lun.", "mar.", etc.). Valor: Número de completados.
    /// </summary>
    public Dictionary<string, int> CompletionsByDayOfWeek { get; set; } = new();
    /// <summary>Clave: Mes en formato "yyyy-MM". Valor: Número de completados.</summary>
    public Dictionary<string, int> CompletionsByMonth { get; set; } = new();
}