using SegundoCerebro.BlazorWasm.Models;
using SegundoCerebro.BlazorWasm.Models.Enums;
using System.Net.Http.Json;

namespace SegundoCerebro.BlazorWasm.Services;

public class TransactionService : ApiService<TransactionDto, CreateTransactionDto, UpdateTransactionDto>, ITransactionService
{
    private new readonly HttpClient _httpClient;
    public TransactionService(HttpClient httpClient) : base(httpClient, "transactions")
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<TransactionDto>> GetByAccountIdAsync(Guid accountId)
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<TransactionDto>>($"api/transactions/account/{accountId}");
        return result ?? Enumerable.Empty<TransactionDto>();
    }

    public async Task<IEnumerable<TransactionDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<TransactionDto>>(
            $"api/transactions/date-range?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
        return result ?? Enumerable.Empty<TransactionDto>();
    }

    public async Task<IEnumerable<TransactionDto>> GetByCategoryIdAsync(Guid categoryId)
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<TransactionDto>>($"api/transactions/category/{categoryId}");
        return result ?? Enumerable.Empty<TransactionDto>();   
    }

    public async Task<decimal> GetAccountBalanceAsync(Guid accountId)
    {
        var transactions = await GetByAccountIdAsync(accountId);
        return transactions.Sum(t => t.Type == TransactionType.Income ? t.Amount : -t.Amount);
    }

}