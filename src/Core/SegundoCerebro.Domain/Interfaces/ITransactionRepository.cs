using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Domain.Interfaces;

public interface ITransactionRepository : IRepository<Transaction>
{
    Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId);
    Task<IEnumerable<Transaction>> GetByCategoryIdAsync(Guid categoryId);
    Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<Transaction?> GetWithDetailsAsync(Guid id);
    Task<decimal> GetTotalIncomeAsync(Guid accountId, DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalExpensesAsync(Guid accountId, DateTime startDate, DateTime endDate);
}