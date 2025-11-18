using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Budgets.Queries.GetBudgetById;

public record GetBudgetByIdQuery(Guid Id) : IRequest<BudgetDto?>;