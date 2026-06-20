using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Application.DTOs;

/// <summary>
/// DTO para representar los datos de un proyecto al ser consultado.
/// </summary>
public class ProjectDto
{
    /// <summary>Identificador único del proyecto.</summary>
    public Guid Id { get; set; }
    /// <summary>Nombre del proyecto.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Descripción detallada del proyecto.</summary>
    public string? Description { get; set; }
    /// <summary>Estado actual del proyecto (ej. Activo, Completado).</summary>
    public ProjectStatus Status { get; set; }
    /// <summary>Fecha de inicio del proyecto.</summary>
    public DateTime? StartDate { get; set; }
    /// <summary>Fecha de finalización del proyecto.</summary>
    public DateTime? EndDate { get; set; }
    /// <summary>Fecha de creación del registro.</summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>Fecha de la última actualización.</summary>
    public DateTime? UpdatedAt { get; set; }
}