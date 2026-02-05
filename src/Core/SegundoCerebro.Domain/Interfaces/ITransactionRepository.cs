// filepath: src/Core/SegundoCerebro.Domain/Interfaces/ITransactionRepository.cs
using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Domain.Interfaces;

public interface ITransactionRepository : IRepository<Transaction>
{
    Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId);
    Task<IEnumerable<Transaction>> GetByCategoryIdAsync(Guid categoryId);
    Task<IEnumerable<Transaction>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);
}