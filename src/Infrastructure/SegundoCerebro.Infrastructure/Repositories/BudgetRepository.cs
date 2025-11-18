using Microsoft.EntityFrameworkCore;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Data;

namespace SegundoCerebro.Infrastructure.Repositories;

public class BudgetRepository : Repository<Budget>, IBudgetRepository
{
    public BudgetRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Budget>> GetActiveBudgetsAsync()
    {
        return await _dbSet
            .Include(b => b.Category)
            .Include(b => b.Account)
            .Where(b => b.IsActive)
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Budget>> GetBudgetsByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(b => b.Category)
            .Include(b => b.Account)
            .Where(b => b.IsActive && 
                       b.StartDate <= endDate && 
                       b.EndDate >= startDate)
            .OrderBy(b => b.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Budget>> GetOverBudgetsAsync()
    {
        return await _dbSet
            .Include(b => b.Category)
            .Include(b => b.Account)
            .Where(b => b.IsActive && b.Spent > b.Amount)
            .OrderByDescending(b => b.Spent - b.Amount)
            .ToListAsync();
    }

    public async Task<Budget?> GetBudgetByCategoryAndPeriodAsync(Guid categoryId, DateTime date)
    {
        return await _dbSet
            .Include(b => b.Category)
            .Include(b => b.Account)
            .FirstOrDefaultAsync(b => b.CategoryId == categoryId &&
                                    b.IsActive &&
                                    b.StartDate <= date &&
                                    b.EndDate >= date);
    }

    public async Task UpdateBudgetSpentAsync(Guid budgetId, decimal amount)
    {
        var budget = await _dbSet.FindAsync(budgetId);
        if (budget != null)
        {
            budget.Spent += amount;
            budget.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(budget);
        }
    }

    public override async Task<IEnumerable<Budget>> GetAllAsync()
    {
        return await _dbSet
            .Include(b => b.Category)
            .Include(b => b.Account)
            .OrderBy(b => b.Name)
            .ToListAsync();
    }

    public override async Task<Budget?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(b => b.Category)
            .Include(b => b.Account)
            .FirstOrDefaultAsync(b => b.Id == id);
    }
}