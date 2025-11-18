using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Reports.Queries.GetFinancialSummary;

public record GetFinancialSummaryQuery(DateTime StartDate, DateTime EndDate) : IRequest<FinancialSummaryDto>;