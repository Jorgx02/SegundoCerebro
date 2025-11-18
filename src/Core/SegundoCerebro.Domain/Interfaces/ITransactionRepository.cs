using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Domain.Interfaces;

public interface ITransactionRepository : IRepository<Transaction>
{
    Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId);
    Task<IEnumerable<Transaction>> GetByCategoryIdAsync(Guid categoryId);
    Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Transaction>> GetByTypeAsync(TransactionType type);
    Task<decimal> GetTotalByTypeAndDateRangeAsync(TransactionType type, DateTime startDate, DateTime endDate);
    Task<IEnumerable<Transaction>> GetRecentTransactionsAsync(int count = 10);
}