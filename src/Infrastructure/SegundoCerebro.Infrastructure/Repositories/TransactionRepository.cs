using Microsoft.EntityFrameworkCore;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;
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
            .Include(t => t.Account)
            .Include(t => t.Category)
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetByCategoryIdAsync(Guid categoryId)
    {
        return await _dbSet
            .Include(t => t.Account)
            .Include(t => t.Category)
            .Where(t => t.CategoryId == categoryId)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(t => t.Account)
            .Include(t => t.Category)
            .Where(t => t.Date >= startDate && t.Date <= endDate)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetByTypeAsync(TransactionType type)
    {
        return await _dbSet
            .Include(t => t.Account)
            .Include(t => t.Category)
            .Where(t => t.Type == type)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalByTypeAndDateRangeAsync(TransactionType type, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(t => t.Type == type && t.Date >= startDate && t.Date <= endDate)
            .SumAsync(t => t.Amount);
    }

    public async Task<IEnumerable<Transaction>> GetRecentTransactionsAsync(int count = 10)
    {
        return await _dbSet
            .Include(t => t.Account)
            .Include(t => t.Category)
            .OrderByDescending(t => t.Date)
            .Take(count)
            .ToListAsync();
    }

    public override async Task<IEnumerable<Transaction>> GetAllAsync()
    {
        return await _dbSet
            .Include(t => t.Account)
            .Include(t => t.Category)
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public override async Task<Transaction?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(t => t.Account)
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}