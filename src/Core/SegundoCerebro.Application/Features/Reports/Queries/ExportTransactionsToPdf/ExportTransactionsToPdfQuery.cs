using MediatR;

namespace SegundoCerebro.Application.Features.Reports.Queries.ExportTransactionsToPdf;

public record ExportTransactionsToPdfQuery(DateTime StartDate, DateTime EndDate, Guid? AccountId = null) : IRequest<byte[]>;