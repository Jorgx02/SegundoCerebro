// filepath: src/Core/SegundoCerebro.Application/Features/Reports/Queries/ExportTransactionsToPdf/ExportTransactionsToPdfQueryHandler.cs
using MediatR;
using SegundoCerebro.Domain.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using MediatRUnit = MediatR.Unit;

namespace SegundoCerebro.Application.Features.Reports.Queries.ExportTransactionsToPdf;

public class ExportTransactionsToPdfQueryHandler : IRequestHandler<ExportTransactionsToPdfQuery, byte[]>
{
    private readonly IUnitOfWork _unitOfWork;

    public ExportTransactionsToPdfQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<byte[]> Handle(ExportTransactionsToPdfQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _unitOfWork.Transactions.GetByDateRangeAsync(request.StartDate, request.EndDate);
        var transactionsList = transactions.OrderByDescending(t => t.Date).ToList();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header()
                    .Text("Transaction Report")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, QuestPDF.Infrastructure.Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Spacing(5);

                        // Period info
                        column.Item().Text($"Period: {request.StartDate:dd/MM/yyyy} - {request.EndDate:dd/MM/yyyy}");
                        column.Item().Text($"Generated: {DateTime.Now:dd/MM/yyyy HH:mm}");

                        column.Item().PaddingTop(10);

                        // Table
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1.5f); // Date
                                columns.RelativeColumn(3);    // Description
                                columns.RelativeColumn(1.5f); // Amount
                                columns.RelativeColumn(1.5f); // Type
                                columns.RelativeColumn(2);    // Category
                                columns.RelativeColumn(2);    // Account
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Date");
                                header.Cell().Element(CellStyle).Text("Description");
                                header.Cell().Element(CellStyle).Text("Amount");
                                header.Cell().Element(CellStyle).Text("Type");
                                header.Cell().Element(CellStyle).Text("Category");
                                header.Cell().Element(CellStyle).Text("Account");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                                }
                            });

                            // Data
                            foreach (var transaction in transactionsList)
                            {
                                table.Cell().Element(CellStyle).Text(transaction.Date.ToString("dd/MM/yyyy"));
                                table.Cell().Element(CellStyle).Text(transaction.Description);
                                table.Cell().Element(CellStyle).Text(transaction.Amount.ToString("C"));
                                table.Cell().Element(CellStyle).Text(transaction.Type.ToString());
                                table.Cell().Element(CellStyle).Text(transaction.Category?.Name ?? "-");
                                table.Cell().Element(CellStyle).Text(transaction.Account?.Name ?? "-");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                                }
                            }
                        });

                        // Summary
                        column.Item().PaddingTop(20);
                        var totalIncome = transactionsList.Where(t => t.Type == Domain.Enums.TransactionType.Income).Sum(t => t.Amount);
                        var totalExpenses = transactionsList.Where(t => t.Type == Domain.Enums.TransactionType.Expense).Sum(t => t.Amount);

                        column.Item().Text("Summary").SemiBold().FontSize(14);
                        column.Item().Text($"Total Income: {totalIncome:C}").FontColor(Colors.Green.Medium);
                        column.Item().Text($"Total Expenses: {totalExpenses:C}").FontColor(Colors.Red.Medium);
                        column.Item().Text($"Net: {(totalIncome - totalExpenses):C}").SemiBold();
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                    });
            });
        });

        return document.GeneratePdf();
    }
}