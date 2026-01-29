using SegundoCerebro.BlazorWasm.Models;
using System.Net.Http.Json;

namespace SegundoCerebro.BlazorWasm.Services;

public class BudgetService : ApiService<BudgetDto, CreateBudgetDto, UpdateBudgetDto>, IBudgetService
{
    private readonly HttpClient _httpClient;

    public BudgetService(HttpClient httpClient) : base(httpClient, "budgets")
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<BudgetDto>> GetActiveBudgetsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<BudgetDto>>("api/budgets/active");
        return result ?? Enumerable.Empty<BudgetDto>();
    }

    public async Task<IEnumerable<BudgetDto>> GetOverBudgetsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<BudgetDto>>("api/budgets/over");
        return result ?? Enumerable.Empty<BudgetDto>();
    }

    public async Task<IEnumerable<BudgetDto>> GetByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<BudgetDto>>(
            $"api/budgets/period?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
        return result ?? Enumerable.Empty<BudgetDto>();
    }
}