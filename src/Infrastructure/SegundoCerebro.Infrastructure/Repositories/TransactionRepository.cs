using Microsoft.EntityFrameworkCore;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Data;

namespace SegundoCerebro.Infrastructure.Repositories;

public class TransactionRepository : Repository<Transaction>, ITransactionRepository
{
    public TransactionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId)
    {
        return await _dbSet
            .Include(t => t.Category)
            .Include(t => t.Account)
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetByCategoryIdAsync(Guid categoryId)
    {
        return await _dbSet
            .Include(t => t.Category)
            .Include(t => t.Account)
            .Where(t => t.CategoryId == categoryId)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(t => t.Category)
            .Include(t => t.Account)
            .Where(t => t.Date >= startDate && t.Date <= endDate)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<Transaction?> GetWithDetailsAsync(Guid id)
    {
        return await _dbSet
            .Include(t => t.Category)
            .Include(t => t.Account)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<decimal> GetTotalIncomeAsync(Guid accountId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(t => t.AccountId == accountId 
                && t.Type == Domain.Enums.TransactionType.Income
                && t.Date >= startDate 
                && t.Date <= endDate)
            .SumAsync(t => t.Amount);
    }

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