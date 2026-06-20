using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Domain.Interfaces;

/// <summary>
/// Define el contrato para el repositorio de Transacciones (Transactions), extendiendo el repositorio genérico.
/// Proporciona métodos de consulta específicos para la entidad Transaction.
/// </summary>
public interface ITransactionRepository : IRepository<Transaction>
{
    /// <summary>
    /// Obtiene una cuenta por su id.
    /// </summary>
    /// <param name="accountId">El ID de la cuenta.</param>
    /// <returns>La cuenta encontrada o null si no se encuentra.</returns>
    Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId);

    /// <summary>
    /// Obtiene una categoría por su id.
    /// </summary>
    /// <param name="categoryId">El ID de la categoría.</param>
    /// <returns>La categoría encontrada o null si no se encuentra.</returns>
    Task<IEnumerable<Transaction>> GetByCategoryIdAsync(Guid categoryId);

    /// <summary>
    /// Obtiene un presupuesto con sus detalles.
    /// </summary>
    /// <param name="startDate">Fecha de inicio.</param>
    /// <param name="endDate">Fecha de fin.</param>
    /// <returns>El presupuesto encontrado o null si no se encuentra.</returns>
    Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Obtiene una transacción con sus detalles.
    /// </summary>
    /// <param name="id">El ID de la transacción.</param>
    /// <returns>La transacción encontrada o null si no se encuentra.</returns>
    Task<Transaction?> GetWithDetailsAsync(Guid id);

    /// <summary>
    /// Obtiene el total de ingresos para una cuenta en un rango de fechas.
    /// </summary>
    /// <param name="accountId">El ID de la cuenta.</param>
    /// <param name="startDate">Fecha de inicio.</param>
    /// <param name="endDate">Fecha de fin.</param>
    /// <returns>El total de ingresos encontrados.</returns>
    Task<decimal> GetTotalIncomeAsync(Guid accountId, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Obtiene el total de gastos para una cuenta en un rango de fechas.
    /// </summary>
    /// <param name="accountId">El ID de la cuenta.</param>
    /// <param name="startDate">Fecha de inicio.</param>
    /// <param name="endDate">Fecha de fin.</param>
    /// <returns>El total de gastos encontrados.</returns>
    Task<decimal> GetTotalExpensesAsync(Guid accountId, DateTime startDate, DateTime endDate);
}