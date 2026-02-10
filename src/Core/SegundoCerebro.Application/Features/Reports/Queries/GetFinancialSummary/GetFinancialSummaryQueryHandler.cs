using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Domain.Enums;
using System.Globalization;

namespace SegundoCerebro.Application.Features.Reports.Queries.GetFinancialSummary;

public class GetFinancialSummaryQueryHandler : IRequestHandler<GetFinancialSummaryQuery, FinancialSummaryDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFinancialSummaryQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<FinancialSummaryDto> Handle(GetFinancialSummaryQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _unitOfWork.Transactions.GetByDateRangeAsync(request.StartDate, request.EndDate);
        var accounts = await _unitOfWork.Accounts.GetAllAsync();

        var summary = new FinancialSummaryDto
        {
            TotalBalance = accounts.Sum(a => a.Balance),
            TotalIncome = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount),
            TotalExpenses = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount),
            ActiveAccounts = accounts.Count(a => a.IsActive),
            TotalTransactions = transactions.Count()
        };

        summary.NetIncome = summary.TotalIncome - summary.TotalExpenses;

        // Category Breakdown
        var expenseTransactions = transactions.Where(t => t.Type == TransactionType.Expense).ToList();
        var totalExpenses = expenseTransactions.Sum(t => t.Amount);

        summary.CategoryBreakdown = expenseTransactions
            .GroupBy(t => new { t.Category.Id, t.Category.Name })
            .Select(g => new CategoryBreakdownDto
            {
                CategoryId = g.Key.Id,
                CategoryName = g.Key.Name,
                Amount = g.Sum(t => t.Amount),
                TransactionCount = g.Count(),
                Percentage = totalExpenses > 0 ? (g.Sum(t => t.Amount) / totalExpenses) * 100 : 0
            })
            .OrderByDescending(c => c.Amount)
            .ToList();

        // Monthly Trends
        summary.MonthlyTrends = transactions
            .GroupBy(t => new { t.Date.Year, t.Date.Month })
            .Select(g => new MonthlyTrendDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                MonthName = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM yyyy", CultureInfo.CurrentCulture),
                Income = g.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount),
                Expenses = g.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount)
            })
            .OrderBy(m => m.Year).ThenBy(m => m.Month)
            .ToList();

        foreach (var trend in summary.MonthlyTrends)
        {
            trend.Net = trend.Income - trend.Expenses;
        }

        return summary;
    }
}