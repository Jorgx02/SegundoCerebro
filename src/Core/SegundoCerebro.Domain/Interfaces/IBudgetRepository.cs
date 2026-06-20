using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Domain.Interfaces;

/// <summary>
/// Define el contrato para el repositorio de Presupuestos (Budget), extendiendo el repositorio genérico.
/// Proporciona métodos de consulta específicos para la entidad Budget.
/// </summary>
public interface IBudgetRepository : IRepository<Budget>
{
    /// <summary>
    /// Obtiene todas las tareas asociadas a un proyecto específico.
    /// </summary>
    /// <returns>Una colección de los presupuestos activos.</returns>
    Task<IEnumerable<Budget>> GetActiveBudgetsAsync();

    /// <summary>
    /// Obtiene todos los presupuestos que exceden su límite.
    /// </summary>
    /// <returns>Una colección de presupuestos que exceden su límite.</returns>
    Task<IEnumerable<Budget>> GetOverBudgetsAsync();

    /// <summary>
    /// Obtiene todos los presupuestos dentro de un período específico.
    /// </summary>
    /// <param name="startDate">La fecha de inicio.</param>
    /// <param name="endDate">La fecha de fin.</param>
    /// <returns>Una colección de presupuestos del período.</returns>
    Task<IEnumerable<Budget>> GetByPeriodAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Obtiene un presupuesto con sus detalles.
    /// </summary>
    /// <param name="id">El ID del presupuesto.</param>
    /// <returns>El presupuesto encontrado o null si no se encuentra.</returns>
    Task<Budget?> GetBudgetWithDetailsAsync(Guid id);

    /// <summary>
    /// Obtiene un presupuesto por categoría y período.
    /// </summary>
    /// <param name="categoryId">El ID de la categoría.</param>
    /// <param name="date">La fecha del período.</param>
    /// <returns>El presupuesto encontrado o null si no se encuentra.</returns>
    Task<Budget?> GetBudgetByCategoryAndPeriodAsync(Guid categoryId, DateTime date);

    /// <summary>
    /// Actualiza el monto gastado de un presupuesto.
    /// </summary>
    /// <param name="budgetId">El ID del presupuesto.</param>
    /// <param name="amount">El monto a actualizar.</param>
    Task UpdateBudgetSpentAsync(Guid budgetId, decimal amount);
}