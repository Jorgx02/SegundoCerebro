using MediatR;
using SegundoCerebro.Domain.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace SegundoCerebro.Application.Features.Reports.Queries.ExportSingleTransactionToPdf;

public class ExportSingleTransactionToPdfQueryHandler : IRequestHandler<ExportSingleTransactionToPdfQuery, byte[]>
{
    private readonly IUnitOfWork _unitOfWork;

    public ExportSingleTransactionToPdfQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<byte[]> Handle(ExportSingleTransactionToPdfQuery request, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.Transactions.GetByIdAsync(request.TransactionId);
        if (transaction == null) throw new Exception("Transaction not found");

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Header()
                    .Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text("Factura / Recibo").SemiBold().FontSize(24).FontColor(Colors.Blue.Darken2);
                            col.Item().Text($"ID: {transaction.Id.ToString().Substring(0, 8).ToUpper()}");
                            col.Item().Text($"Fecha: {transaction.Date:dd/MM/yyyy}");
                        });
                    });

                page.Content()
                    .PaddingVertical(1, QuestPDF.Infrastructure.Unit.Centimetre)
                    .Column(col =>
                    {
                        col.Spacing(15);
                        
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(120);
                                columns.RelativeColumn();
                            });
                            
                            table.Cell().Text("Tipo:");
                            table.Cell().Text(transaction.Type.ToString()).SemiBold();
                            
                            table.Cell().Text("Cuenta:");
                            table.Cell().Text(transaction.Account?.Name ?? "-").SemiBold();
                            
                            table.Cell().Text("Categoría:");
                            table.Cell().Text(transaction.Category?.Name ?? "-").SemiBold();
                            
                            table.Cell().Text("Monto:");
                            table.Cell().Text(transaction.Amount.ToString("C2")).SemiBold().FontSize(14).FontColor(transaction.Type == Domain.Enums.TransactionType.Income ? Colors.Green.Medium : Colors.Red.Medium);
                            
                            table.Cell().Text("Descripción:");
                            table.Cell().Text(transaction.Description);
                        });
                    });

                page.Footer()
                    .AlignCenter()
                    .Text("Generado por SegundoCerebro").FontSize(9).FontColor(Colors.Grey.Medium);
            });
        });

        return document.GeneratePdf();
    }
}
