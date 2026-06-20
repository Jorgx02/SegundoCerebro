using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Application.DTOs;

/// <summary>
/// DTO utilizado para actualizar un proyecto existente.
/// </summary>
public class UpdateProjectDto
{
    /// <summary>Nuevo nombre del proyecto.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Nueva descripción opcional.</summary>
    public string? Description { get; set; }
    /// <summary>Nuevo estado del proyecto.</summary>
    public ProjectStatus Status { get; set; }
    /// <summary>Nueva fecha de inicio.</summary>
    public DateTime? StartDate { get; set; }
    /// <summary>Nueva fecha de finalización.</summary>
    public DateTime? EndDate { get; set; }
}