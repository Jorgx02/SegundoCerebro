using SegundoCerebro.BlazorWasm.Models;
using System.Net.Http.Json;

namespace SegundoCerebro.BlazorWasm.Services;

public class ReportService : IReportService
{
    private readonly HttpClient _httpClient;

    public ReportService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<FinancialSummaryDto> GetFinancialSummaryAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = $"api/reports/financial-summary";
        
        if (startDate.HasValue && endDate.HasValue)
        {
            query += $"?startDate={startDate.Value:yyyy-MM-dd}&endDate={endDate.Value:yyyy-MM-dd}";
        }

        var result = await _httpClient.GetFromJsonAsync<FinancialSummaryDto>(query);
        return result ?? new FinancialSummaryDto();
    }

    public async Task<byte[]> ExportTransactionsToExcelAsync(DateTime startDate, DateTime endDate)
    {
        var response = await _httpClient.GetAsync(
            $"api/reports/export/excel?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
        
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync();
    }

    public async Task<byte[]> ExportTransactionsToPdfAsync(DateTime startDate, DateTime endDate)
    {
        var response = await _httpClient.GetAsync(
            $"api/reports/export/pdf?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
        
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync();
    }
}