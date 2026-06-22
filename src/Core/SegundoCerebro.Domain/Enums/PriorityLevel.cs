namespace SegundoCerebro.Domain.Enums;

/// <summary>
/// Define los niveles de prioridad para una tarea (`TodoItem`), ayudando a decidir qué hacer a continuación.
/// </summary>
public enum PriorityLevel
{
    /// <summary>Prioridad baja, no es urgente ni importante.</summary>
    Low = 0,

    /// <summary>Prioridad por defecto, normal.</summary>
    Medium = 1,

    /// <summary>Prioridad alta, debe atenderse pronto.</summary>
    High = 2,

    /// <summary>Prioridad crítica, requiere atención inmediata.</summary>
    Critical = 3
}