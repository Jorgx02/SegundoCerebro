namespace SegundoCerebro.Domain.Entities;

/// <summary>
/// Representa un registro de cumplimiento de un hábito en una fecha específica.
/// </summary>
public class HabitLog
{
    /// <summary>Identificador único del registro.</summary>
    public Guid Id { get; set; }

    /// <summary>La fecha en la que se completó el hábito. Se almacena solo la parte de la fecha (sin hora).</summary>
    public DateTime Date { get; set; }

    /// <summary>Indica si el hábito se completó en esa fecha.</summary>
    public bool IsCompleted { get; set; } = true;

    /// <summary>Notas opcionales sobre el cumplimiento del hábito en ese día.</summary>
    public string? Notes { get; set; }

    // Foreign Key
    public Guid HabitId { get; set; }

    // Navigation Property
    public Habit Habit { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;
}