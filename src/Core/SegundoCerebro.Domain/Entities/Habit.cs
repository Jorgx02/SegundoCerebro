using SegundoCerebro.Domain.Enums;
using System.Collections.Generic;

namespace SegundoCerebro.Domain.Entities;

/// <summary>
/// Representa un hábito que el usuario desea seguir.
/// </summary>
public class Habit
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public HabitFrequency Frequency { get; set; }
    public string? Icon { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Colección de registros de cumplimiento para este hábito.
    /// </summary>
    public ICollection<HabitLog> Logs { get; set; } = new List<HabitLog>();
}