using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Domain.Interfaces;

/// <summary>
/// Define el contrato para el repositorio de Registros de Tiempo (TimeLog).
/// </summary>
public interface ITimeLogRepository : IRepository<TimeLog>
{
    Task<TimeLog?> GetActiveLogForTaskAsync(Guid todoItemId);
}