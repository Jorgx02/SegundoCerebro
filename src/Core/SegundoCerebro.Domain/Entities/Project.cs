using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Domain.Entities;

/// <summary>
/// Representa un proyecto o iniciativa que puede contener múltiples tareas (TodoItems) y tiene un estado de progreso.
/// </summary>
public class Project
{
    /// <summary>Identificador delcproyecto.</summary>
    public Guid Id { get; set; }

    /// <summary>Nombre del proyecto.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Descripción del proyecto.</summary>
    public string? Description { get; set; }

    /// <summary>Estado en el que se encuentra el proyecto.</summary>
    public ProjectStatus Status { get; set; } = ProjectStatus.NotStarted;

    /// <summary>Fecha de inicio del proyecto.</summary>
    public DateTime? StartDate { get; set; }

    /// <summary>Fecha de finalización del proyecto.</summary>
    public DateTime? EndDate { get; set; }

    /// <summary>Fecha de creación del proyecto.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Fecha de la última actualización del proyecto.</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Id del usuario propietario del proyecto.</summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>Tareas asociadas a un proyecto.</summary>
    public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
}