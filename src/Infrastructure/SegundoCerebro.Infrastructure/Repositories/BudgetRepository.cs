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
        return await _context.Budgets
            .Include(b => b.Category)
            .Include(b => b.Account)
            .Where(b => b.IsActive && b.EndDate >= DateTime.Now)
            .OrderBy(b => b.Category.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Budget>> GetOverBudgetsAsync()
    {
        return await _context.Budgets
            .Include(b => b.Category)
            .Include(b => b.Account)
            .Where(b => b.IsActive && b.Spent > b.Amount)
            .OrderByDescending(b => b.Spent - b.Amount)
            .ToListAsync();
    }

    public async Task<IEnumerable<Budget>> GetByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Budgets
            .Include(b => b.Category)
            .Include(b => b.Account)
            .Where(b => b.StartDate >= startDate && b.EndDate <= endDate)
            .OrderBy(b => b.StartDate)
            .ToListAsync();
    }

    public async Task<Budget?> GetBudgetWithDetailsAsync(Guid id)
    {
        return await _context.Budgets
            .Include(b => b.Category)
            .Include(b => b.Account)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Budget?> GetBudgetByCategoryAndPeriodAsync(Guid categoryId, DateTime date)
    {
        return await _context.Budgets
            .Include(b => b.Category)
            .Include(b => b.Account)
            .Where(b => b.CategoryId == categoryId
                     && b.IsActive
                     && b.StartDate <= date
                     && b.EndDate >= date)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateBudgetSpentAsync(Guid budgetId, decimal amount)
    {
        var budget = await _context.Budgets.FindAsync(budgetId);
        if (budget != null)
        {
            budget.Spent += amount;
            budget.UpdatedAt = DateTime.UtcNow;
            _context.Budgets.Update(budget);
        }
        await Task.CompletedTask;
    }
}