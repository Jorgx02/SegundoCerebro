namespace SegundoCerebro.Application.DTOs;

/// <summary>
/// DTO utilizado para crear un nuevo proyecto.
/// </summary>
public class CreateProjectDto
{
    /// <summary>Nombre del proyecto.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Descripción detallada opcional del proyecto.</summary>
    public string? Description { get; set; }
    /// <summary>Fecha de inicio estimada del proyecto.</summary>
    public DateTime? StartDate { get; set; }
    /// <summary>Fecha de finalización estimada del proyecto.</summary>
    public DateTime? EndDate { get; set; }
}