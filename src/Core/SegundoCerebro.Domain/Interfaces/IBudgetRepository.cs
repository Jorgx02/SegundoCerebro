using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Domain.Interfaces;

public interface IBudgetRepository : IRepository<Budget>
{
    Task<IEnumerable<Budget>> GetActiveBudgetsAsync();
    Task<IEnumerable<Budget>> GetOverBudgetsAsync();
    Task<IEnumerable<Budget>> GetByPeriodAsync(DateTime startDate, DateTime endDate);
    Task<Budget?> GetBudgetWithDetailsAsync(Guid id);
    Task<Budget?> GetBudgetByCategoryAndPeriodAsync(Guid categoryId, DateTime date);
    Task UpdateBudgetSpentAsync(Guid budgetId, decimal amount);
}