namespace SegundoCerebro.Domain.Enums;

/// <summary>
/// Define los estados de una tarea (`TodoItem`) según la metodología GTD (Getting Things Done).
/// </summary>
public enum TodoItemStatus
{
    /// <summary>Bandeja de entrada: La tarea ha sido capturada pero aún no ha sido procesada o clarificada.</summary>
    Inbox = 0,

    /// <summary>Próxima Acción: Tarea clara y lista para ser ejecutada tan pronto como sea posible.</summary>
    NextAction = 1,

    /// <summary>En Espera: La tarea ha sido delegada o se está esperando un evento externo para poder continuar.</summary>
    WaitingFor = 2,

    /// <summary>Programada: La tarea tiene una fecha y hora específicas para ser realizada (cita en calendario).</summary>
    Scheduled = 3,

    /// <summary>Completada: La tarea ha sido finalizada.</summary>
    Completed = 4,

    /// <summary>Algún día/Tal vez: Idea o tarea que no se va a realizar ahora, pero que se quiere guardar para el futuro.</summary>
    Someday = 5
}