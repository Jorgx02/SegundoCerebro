using SegundoCerebro.BlazorWasm.Models;
using System.Net.Http.Json;

namespace SegundoCerebro.BlazorWasm.Services;

public class BudgetService : ApiService<BudgetDto, CreateBudgetDto, UpdateBudgetDto>, IBudgetService
{
    public BudgetService(HttpClient httpClient) : base(httpClient, "budgets")
    {
    }

    public async Task<IEnumerable<BudgetDto>> GetActiveBudgetsAsync()
    {
        var allBudgets = await GetAllAsync();
        return allBudgets.Where(b => b.IsActive);
    }

    public async Task<IEnumerable<BudgetDto>> GetOverBudgetsAsync()
    {
        var allBudgets = await GetAllAsync();
        return allBudgets.Where(b => b.IsOverBudget);
    }

    public async Task<IEnumerable<BudgetDto>> GetByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        var allBudgets = await GetAllAsync();
        return allBudgets.Where(b => b.StartDate >= startDate && b.EndDate <= endDate);
    }
}