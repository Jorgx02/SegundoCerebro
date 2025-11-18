using SegundoCerebro.BlazorWasm.Models;
using SegundoCerebro.BlazorWasm.Models.Enums;
using System.Text.Json;

namespace SegundoCerebro.BlazorWasm.Services;

public class TransactionService : ApiService<TransactionDto, CreateTransactionDto, UpdateTransactionDto>, ITransactionService
{
    public TransactionService(HttpClient httpClient) : base(httpClient, "transactions")
    {
    }

    public async Task<IEnumerable<TransactionDto>> GetByAccountAsync(Guid accountId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/{_endpoint}/account/{accountId}");
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<TransactionDto>>(json, _jsonOptions) ?? [];
        }
        catch
        {
            return [];
        }
    }

    public async Task<IEnumerable<TransactionDto>> GetByCategoryAsync(Guid categoryId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/{_endpoint}/category/{categoryId}");
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<TransactionDto>>(json, _jsonOptions) ?? [];
        }
        catch
        {
            return [];
        }
    }

    public async Task<IEnumerable<TransactionDto>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/{_endpoint}/daterange?start={startDate:yyyy-MM-dd}&end={endDate:yyyy-MM-dd}");
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<TransactionDto>>(json, _jsonOptions) ?? [];
        }
        catch
        {
            return [];
        }
    }

    public async Task<decimal> GetAccountBalanceAsync(Guid accountId)
    {
        try
        {
            var transactions = await GetByAccountAsync(accountId);
            return transactions.Sum(t => t.Type == TransactionType.Income ? t.Amount : -t.Amount);
        }
        catch
        {
            return 0;
        }
    }
}