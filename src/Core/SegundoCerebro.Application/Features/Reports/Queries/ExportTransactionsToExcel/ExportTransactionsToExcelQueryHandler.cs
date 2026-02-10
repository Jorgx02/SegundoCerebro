using MediatR;
using SegundoCerebro.Domain.Interfaces;
using ClosedXML.Excel;
using System.Globalization;

namespace SegundoCerebro.Application.Features.Reports.Queries.ExportTransactionsToExcel;

public class ExportTransactionsToExcelQueryHandler : IRequestHandler<ExportTransactionsToExcelQuery, byte[]>
{
    private readonly IUnitOfWork _unitOfWork;

    public ExportTransactionsToExcelQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<byte[]> Handle(ExportTransactionsToExcelQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _unitOfWork.Transactions.GetByDateRangeAsync(request.StartDate, request.EndDate);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Transactions");

        // Headers
        worksheet.Cell(1, 1).Value = "Date";
        worksheet.Cell(1, 2).Value = "Description";
        worksheet.Cell(1, 3).Value = "Amount";
        worksheet.Cell(1, 4).Value = "Type";
        worksheet.Cell(1, 5).Value = "Category";
        worksheet.Cell(1, 6).Value = "Account";
        worksheet.Cell(1, 7).Value = "Reference";
        worksheet.Cell(1, 8).Value = "Notes";

        // Style headers
        var headerRange = worksheet.Range(1, 1, 1, 8);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

        // Data
        int row = 2;
        foreach (var transaction in transactions.OrderByDescending(t => t.Date))
        {
            worksheet.Cell(row, 1).Value = transaction.Date.ToString("dd/MM/yyyy");
            worksheet.Cell(row, 2).Value = transaction.Description;
            worksheet.Cell(row, 3).Value = transaction.Amount;
            worksheet.Cell(row, 4).Value = transaction.Type.ToString();
            worksheet.Cell(row, 5).Value = transaction.Category?.Name ?? "-";
            worksheet.Cell(row, 6).Value = transaction.Account?.Name ?? "-";
            worksheet.Cell(row, 7).Value = transaction.Reference ?? "-";
            worksheet.Cell(row, 8).Value = transaction.Notes ?? "-";

            // Color code by type
            if (transaction.Type == Domain.Enums.TransactionType.Income)
            {
                worksheet.Cell(row, 3).Style.Font.FontColor = XLColor.Green;
            }
            else
            {
                worksheet.Cell(row, 3).Style.Font.FontColor = XLColor.Red;
            }

            row++;
        }

        // Auto-fit columns
        worksheet.Columns().AdjustToContents();

        // Add summary at the bottom
        row += 2;
        worksheet.Cell(row, 1).Value = "SUMMARY";
        worksheet.Cell(row, 1).Style.Font.Bold = true;
        
        row++;
        var totalIncome = transactions.Where(t => t.Type == Domain.Enums.TransactionType.Income).Sum(t => t.Amount);
        var totalExpenses = transactions.Where(t => t.Type == Domain.Enums.TransactionType.Expense).Sum(t => t.Amount);
        
        worksheet.Cell(row, 1).Value = "Total Income:";
        worksheet.Cell(row, 2).Value = totalIncome;
        worksheet.Cell(row, 2).Style.Font.FontColor = XLColor.Green;
        
        row++;
        worksheet.Cell(row, 1).Value = "Total Expenses:";
        worksheet.Cell(row, 2).Value = totalExpenses;
        worksheet.Cell(row, 2).Style.Font.FontColor = XLColor.Red;
        
        row++;
        worksheet.Cell(row, 1).Value = "Net:";
        worksheet.Cell(row, 2).Value = totalIncome - totalExpenses;
        worksheet.Cell(row, 1).Style.Font.Bold = true;
        worksheet.Cell(row, 2).Style.Font.Bold = true;

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}