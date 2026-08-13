namespace SegundoCerebro.BlazorWasm.Models;

/// <summary>
/// DTO para representar un registro de cumplimiento de un hábito.
/// </summary>
public class HabitLogDto
{
    /// <summary>Identificador único del registro.</summary>
    public Guid Id { get; set; }
    /// <summary>Fecha en la que se completó el hábito.</summary>
    public DateTime Date { get; set; }
    /// <summary>ID del hábito al que pertenece este registro.</summary>
    public Guid HabitId { get; set; }
}