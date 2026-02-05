using SegundoCerebro.BlazorWasm.Models;

namespace SegundoCerebro.BlazorWasm.Services;

public interface IReportService
{
    Task<FinancialSummaryDto> GetFinancialSummaryAsync(DateTime? startDate = null, DateTime? endDate = null);
    Task<byte[]> ExportTransactionsToExcelAsync(DateTime startDate, DateTime endDate);
    Task<byte[]> ExportTransactionsToPdfAsync(DateTime startDate, DateTime endDate);
}