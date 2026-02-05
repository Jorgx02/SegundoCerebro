using MediatR;

namespace SegundoCerebro.Application.Features.Reports.Queries.ExportTransactionsToExcel;

public record ExportTransactionsToExcelQuery(DateTime StartDate, DateTime EndDate) : IRequest<byte[]>;