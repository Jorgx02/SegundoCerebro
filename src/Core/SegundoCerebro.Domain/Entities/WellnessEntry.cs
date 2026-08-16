namespace SegundoCerebro.Domain.Entities;

/// <summary>
/// Representa una entrada en el diario de bienestar del usuario para una fecha específica.
/// </summary>
public class WellnessEntry
{
    public Guid Id { get; set; }

    /// <summary>Fecha de la entrada del diario.</summary>
    public DateTime Date { get; set; }

    /// <summary>Calificación del estado de ánimo (ej. 1-5).</summary>
    public int MoodRating { get; set; }

    /// <summary>Calificación del nivel de energía (ej. 1-5).</summary>
    public int EnergyLevel { get; set; }

    /// <summary>Notas o reflexiones del día.</summary>
    public string? Notes { get; set; }

    /// <summary>Identificador del propietario (Multi-tenancy).</summary>
    public string UserId { get; set; } = string.Empty;
}