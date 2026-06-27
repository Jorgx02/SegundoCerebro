namespace SegundoCerebro.Domain.Enums;

/// <summary>
/// Define los posibles estados de una tarea, siguiendo la metodología GTD.
/// </summary>
public enum TodoItemStatus
{
    Inbox = 1,      // Tarea capturada, sin procesar.
    NextAction = 2, // Tarea procesada, lista para ser ejecutada.
    InProgress = 3, // Tarea en ejecución.
    WaitingFor = 4, // Tarea delegada o en espera de un evento externo.
    Completed = 5   // Tarea finalizada.
}