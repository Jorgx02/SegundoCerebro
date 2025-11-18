using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;
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
        var accounts = await _unitOfWork.Accounts.GetActiveAccountsAsync();
        var transactions = await _unitOfWork.Transactions.GetByDateRangeAsync(request.StartDate, request.EndDate);
        
        var totalBalance = await _unitOfWork.Accounts.GetTotalBalanceAsync();
        var totalIncome = await _unitOfWork.Transactions.GetTotalByTypeAndDateRangeAsync(
            TransactionType.Income, request.StartDate, request.EndDate);
        var totalExpenses = await _unitOfWork.Transactions.GetTotalByTypeAndDateRangeAsync(
            TransactionType.Expense, request.StartDate, request.EndDate);

        var categoryBreakdown = transactions
            .Where(t => t.Type == TransactionType.Expense)
            .GroupBy(t => new { t.CategoryId, t.Category.Name })
            .Select(g => new CategorySummaryDto
            {
                CategoryId = g.Key.CategoryId,
                CategoryName = g.Key.Name,
                Amount = g.Sum(t => t.Amount),
                TransactionCount = g.Count(),
                Percentage = totalExpenses > 0 ? (g.Sum(t => t.Amount) / totalExpenses) * 100 : 0
            })
            .OrderByDescending(c => c.Amount)
            .ToList();

        var monthlyTrends = transactions
            .GroupBy(t => new { t.Date.Year, t.Date.Month })
            .Select(g => new MonthlyTrendDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month),
                Income = g.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount),
                Expenses = g.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount),
                Net = g.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount) - 
                      g.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount)
            })
            .OrderBy(m => m.Year)
            .ThenBy(m => m.Month)
            .ToList();

        return new FinancialSummaryDto
        {
            TotalBalance = totalBalance,
            TotalIncome = totalIncome,
            TotalExpenses = totalExpenses,
            NetIncome = totalIncome - totalExpenses,
            ActiveAccounts = accounts.Count(),
            TotalTransactions = transactions.Count(),
            CategoryBreakdown = categoryBreakdown,
            MonthlyTrends = monthlyTrends
        };
    }
}