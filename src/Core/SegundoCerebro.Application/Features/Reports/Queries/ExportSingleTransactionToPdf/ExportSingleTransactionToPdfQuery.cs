using MediatR;
using System;

namespace SegundoCerebro.Application.Features.Reports.Queries.ExportSingleTransactionToPdf;

public record ExportSingleTransactionToPdfQuery(Guid TransactionId) : IRequest<byte[]>;
