using SegundoCerebro.BlazorWasm.Models;

namespace SegundoCerebro.BlazorWasm.Services;

public interface IBudgetService : IApiService<BudgetDto, CreateBudgetDto, UpdateBadgetDto>
{
    Task<IEnumerable<BudgetDto>> GetActiveBudgetsAsync();
    Task<IEnumerable<BudgetDto>> GetOverBudgetsAsync();
    Task<IEnumerable<BudgetDto>> GetByPeriodAsync(DateTime startDate, DateTime endDate);
}