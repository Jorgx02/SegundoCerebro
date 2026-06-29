using SegundoCerebro.Domain.Enums;

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
    public string? Color { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; } = string.Empty;

    // Futuro: public ICollection<HabitLog> Logs { get; set; } = new List<HabitLog>();
}