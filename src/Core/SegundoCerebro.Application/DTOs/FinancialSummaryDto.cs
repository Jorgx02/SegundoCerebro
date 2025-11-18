namespace SegundoCerebro.Application.DTOs;

public class FinancialSummaryDto
{
    public decimal TotalBalance { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal NetIncome { get; set; }
    public int ActiveAccounts { get; set; }
    public int TotalTransactions { get; set; }
    public List<CategorySummaryDto> CategoryBreakdown { get; set; } = new();
    public List<MonthlyTrendDto> MonthlyTrends { get; set; } = new();
}

public class CategorySummaryDto
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int TransactionCount { get; set; }
    public decimal Percentage { get; set; }
}