using SegundoCerebro.BlazorWasm.Models.Enums;

namespace SegundoCerebro.BlazorWasm.Models;

/// <summary>
/// DTO para representar los datos de un hábito al ser consultado.
/// </summary>
public class HabitDto
{
    /// <summary>Identificador único del hábito.</summary>
    public Guid Id { get; set; }
    /// <summary>Nombre del hábito.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Descripción opcional del hábito.</summary>
    public string? Description { get; set; }
    /// <summary>Frecuencia con la que se debe realizar el hábito.</summary>
    public HabitFrequency Frequency { get; set; }
    /// <summary>Nombre del icono de MudBlazor asociado.</summary>
    public string? Icon { get; set; }
    /// <summary>Fecha de creación del hábito.</summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>Racha actual de días consecutivos completando el hábito.</summary>
    public int CurrentStreak { get; set; }
    /// <summary>Racha más larga de días consecutivos completando el hábito.</summary>
    public int LongestStreak { get; set; }
    /// <summary>Colección de registros de cumplimiento para este hábito en un rango de fechas específico.</summary>
    public ICollection<HabitLogDto> Logs { get; set; } = new List<HabitLogDto>();
}

/// <summary>
/// DTO utilizado para crear un nuevo hábito.
/// </summary>
public class CreateHabitDto
{
    /// <summary>Nombre del nuevo hábito.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Descripción opcional.</summary>
    public string? Description { get; set; }
    /// <summary>Frecuencia del hábito.</summary>
    public HabitFrequency Frequency { get; set; }
    /// <summary>Icono opcional.</summary>
    public string? Icon { get; set; }
}

/// <summary>
/// DTO utilizado para actualizar un hábito existente.
/// </summary>
public class UpdateHabitDto
{
    /// <summary>Nuevo nombre para el hábito.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Nueva descripción opcional.</summary>
    public string? Description { get; set; }
    /// <summary>Nueva frecuencia para el hábito.</summary>
    public HabitFrequency Frequency { get; set; }
    /// <summary>Nuevo icono opcional.</summary>
    public string? Icon { get; set; }
}