using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Domain.Interfaces;

public interface IBudgetRepository : IRepository<Budget>
{
    Task<IEnumerable<Budget>> GetActiveBudgetsAsync();
    Task<IEnumerable<Budget>> GetBudgetsByPeriodAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Budget>> GetOverBudgetsAsync();
    Task<Budget?> GetBudgetByCategoryAndPeriodAsync(Guid categoryId, DateTime date);
    Task UpdateBudgetSpentAsync(Guid budgetId, decimal amount);
}