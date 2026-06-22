using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Domain.Entities;

/// <summary>
/// Representa un proyecto o una iniciativa a gran escala que agrupa un conjunto de tareas (`TodoItem`).
/// </summary>
public class Project
{
    /// <summary>Identificador único del proyecto.</summary>
    public Guid Id { get; set; }

    /// <summary>Nombre descriptivo del proyecto (ej. "TFG - Memoria Final").</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Descripción detallada opcional sobre los objetivos y alcance del proyecto.</summary>
    public string? Description { get; set; }

    /// <summary>Estado actual del ciclo de vida del proyecto (ej. Activo, En Pausa, Completado).</summary>
    public ProjectStatus Status { get; set; } = ProjectStatus.NotStarted;

    /// <summary>Fecha de inicio estimada o real del proyecto.</summary>
    public DateTime? StartDate { get; set; }

    /// <summary>Fecha de finalización estimada o real del proyecto.</summary>
    public DateTime? EndDate { get; set; }

    /// <summary>Fecha de creación del registro.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Fecha de la última actualización del registro.</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Identificador del usuario propietario del proyecto (Multi-tenancy).</summary>
    public string UserId { get; set; } = string.Empty;

    // Propiedad de Navegación (EF Core)
    /// <summary>Colección de tareas que pertenecen a este proyecto.</summary>
    public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
}