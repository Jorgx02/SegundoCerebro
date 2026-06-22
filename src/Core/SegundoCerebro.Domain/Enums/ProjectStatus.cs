namespace SegundoCerebro.Domain.Enums;

/// <summary>
/// Define el estado del ciclo de vida de un proyecto.
/// </summary>
public enum ProjectStatus
{
    /// <summary>El proyecto ha sido planificado pero no ha comenzado.</summary>
    NotStarted = 0,

    /// <summary>El proyecto está en curso.</summary>
    Active = 1,

    /// <summary>El proyecto está temporalmente pausado.</summary>
    OnHold = 2,

    /// <summary>El proyecto ha sido completado con éxito.</summary>
    Completed = 3,

    /// <summary>El proyecto ha sido cancelado antes de su finalización.</summary>
    Cancelled = 4
}