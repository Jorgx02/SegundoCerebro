using Microsoft.EntityFrameworkCore;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Data;

namespace SegundoCerebro.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio para la gestión de las transacciones financieras en PostgreSQL.
/// </summary>
public class TransactionRepository : Repository<Transaction>, ITransactionRepository
{
    public TransactionRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtiene todas las transacciones asociadas a una cuenta específica, 
    /// incluyendo la información de la categoría y la cuenta asociada.
    /// </summary>
    /// <param name="accountId">Identificador único de la cuenta.</param>
    /// <returns>Lista de transacciones ordenadas descendentemente por fecha.</returns>
    public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId)
    {
        return await _dbSet
            .Include(t => t.Category)
            .Include(t => t.Account)
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene todas las transacciones pertenecientes a una categoría específica,
    /// incluyendo sus datos relacionales.
    /// </summary>
    /// <param name="categoryId">Identificador único de la categoría.</param>
    /// <returns>Lista de transacciones ordenadas descendentemente por fecha.</returns>
    public async Task<IEnumerable<Transaction>> GetByCategoryIdAsync(Guid categoryId)
    {
        return await _dbSet
            .Include(t => t.Category)
            .Include(t => t.Account)
            .Where(t => t.CategoryId == categoryId)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene las transacciones registradas dentro de un rango de fechas determinado.
    /// </summary>
    /// <param name="startDate">Fecha de inicio del filtro.</param>
    /// <param name="endDate">Fecha de fin del filtro.</param>
    /// <returns>Lista de transacciones ordenadas descendentemente por fecha.</returns>
    public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(t => t.Category)
            .Include(t => t.Account)
            .Where(t => t.Date >= startDate && t.Date <= endDate)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    /// <summary>
    /// Sobrescribe el método base para incluir siempre la Cuenta y la Categoría 
    /// asociadas al consultar todas las transacciones, ordenándolas por la más reciente.
    /// </summary>
    public override async Task<IEnumerable<Transaction>> GetAllAsync()
    {
        return await _dbSet
            .Include(t => t.Category)
            .Include(t => t.Account)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene una transacción específica por su ID incluyendo sus tablas relacionales.
    /// </summary>
    /// <param name="id">Identificador único de la transacción.</param>
    /// <returns>La transacción con sus detalles o null si no se encuentra.</returns>
    public async Task<Transaction?> GetWithDetailsAsync(Guid id)
    {
        return await _dbSet
            .Include(t => t.Category)
            .Include(t => t.Account)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <summary>
    /// Calcula la suma total de ingresos (Income) de una cuenta en un rango de fechas.
    /// </summary>
    /// <param name="accountId">Identificador de la cuenta.</param>
    /// <param name="startDate">Fecha inicial.</param>
    /// <param name="endDate">Fecha final.</param>
    /// <returns>El total sumado de ingresos.</returns>
    public async Task<decimal> GetTotalIncomeAsync(Guid accountId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(t => t.AccountId == accountId
                && t.Type == Domain.Enums.TransactionType.Income
                && t.Date >= startDate
                && t.Date <= endDate)
            .SumAsync(t => t.Amount);
    }

    /// <summary>
    /// Calcula la suma total de gastos (Expense) de una cuenta en un rango de fechas.
    /// </summary>
    /// <param name="accountId">Identificador de la cuenta.</param>
    /// <param name="startDate">Fecha inicial.</param>
    /// <param name="endDate">Fecha final.</param>
    /// <returns>El total sumado de gastos.</returns>
    public async Task<decimal> GetTotalExpensesAsync(Guid accountId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(t => t.AccountId == accountId
                && t.Type == Domain.Enums.TransactionType.Expense
                && t.Date >= startDate
                && t.Date <= endDate)
            .SumAsync(t => t.Amount);
    }
}