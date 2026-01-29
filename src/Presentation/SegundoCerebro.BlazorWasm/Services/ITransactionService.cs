using SegundoCerebro.BlazorWasm.Models;

namespace SegundoCerebro.BlazorWasm.Services;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDto>> GetAllAsync();
    Task<TransactionDto?> GetByIdAsync(Guid id);
    Task<TransactionDto> CreateAsync(CreateTransactionDto createDto);
    Task<TransactionDto> UpdateAsync(Guid id, UpdateTransactionDto updateDto);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<TransactionDto>> GetByAccountIdAsync(Guid accountId);
    Task<IEnumerable<TransactionDto>> GetByCategoryIdAsync(Guid categoryId);
    Task<IEnumerable<TransactionDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<decimal> GetAccountBalanceAsync(Guid accountId);
}