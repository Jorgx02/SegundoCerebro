namespace SegundoCerebro.Domain.Enums;

public enum TodoItemStatus
{
    Inbox = 0,       // Idea recién capturada, sin procesar
    NextAction = 1,  // Lista para hacerse cuanto antes
    WaitingFor = 2,  // Delegada o esperando a un tercero
    Scheduled = 3,   // Tiene fecha específica en calendario
    Completed = 4,   // Finalizada
    Someday = 5      // Algún día / Tal vez
}