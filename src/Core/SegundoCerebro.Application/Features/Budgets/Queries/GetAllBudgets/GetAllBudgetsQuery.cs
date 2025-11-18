using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Budgets.Queries.GetAllBudgets;

public record GetAllBudgetsQuery : IRequest<IEnumerable<BudgetDto>>;