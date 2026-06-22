using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Domain.Interfaces;

/// <summary>
/// Define el contrato para el repositorio de Transacciones (`Transaction`), extendiendo el repositorio genérico.
/// Proporciona métodos de consulta específicos para la entidad Transaction.
/// </summary>
public interface ITransactionRepository : IRepository<Transaction>
{
    /// <summary>
    /// Obtiene todas las transacciones asociadas a una cuenta específica.
    /// </summary>
    /// <param name="accountId">El ID de la cuenta.</param>
    /// <returns>Una colección de transacciones de la cuenta especificada.</returns>
    Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId);

    /// <summary>
    /// Obtiene todas las transacciones asociadas a una categoría específica.
    /// </summary>
    /// <param name="categoryId">El ID de la categoría.</param>
    /// <returns>Una colección de transacciones de la categoría especificada.</returns>
    Task<IEnumerable<Transaction>> GetByCategoryIdAsync(Guid categoryId);

    /// <summary>
    /// Obtiene todas las transacciones dentro de un rango de fechas.
    /// </summary>
    /// <param name="startDate">La fecha de inicio del rango.</param>
    /// <param name="endDate">La fecha de fin del rango.</param>
    /// <returns>Una colección de transacciones dentro del rango de fechas.</returns>
    Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Obtiene una transacción incluyendo sus entidades relacionadas (Cuenta y Categoría).
    /// </summary>
    /// <param name="id">El ID de la transacción.</param>
    /// <returns>La transacción con sus detalles, o null si no se encuentra.</returns>
    Task<Transaction?> GetWithDetailsAsync(Guid id);

    /// <summary>
    /// Obtiene el total de ingresos para una cuenta en un rango de fechas.
    /// </summary>
    /// <param name="accountId">El ID de la cuenta.</param>
    /// <param name="startDate">La fecha de inicio del rango.</param>
    /// <param name="endDate">La fecha de fin del rango.</param>
    /// <returns>La suma total de los ingresos.</returns>
    Task<decimal> GetTotalIncomeAsync(Guid accountId, DateTime startDate, DateTime endDate);

    /// <summary>
    /// Obtiene el total de gastos para una cuenta en un rango de fechas.
    /// </summary>
    /// <param name="accountId">El ID de la cuenta.</param>
    /// <param name="startDate">La fecha de inicio del rango.</param>
    /// <param name="endDate">La fecha de fin del rango.</param>
    /// <returns>La suma total de los gastos.</returns>
    Task<decimal> GetTotalExpensesAsync(Guid accountId, DateTime startDate, DateTime endDate);
}