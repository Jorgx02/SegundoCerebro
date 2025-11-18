using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Budgets.Commands.UpdateBudget;

public record UpdateBudgetCommand(Guid Id, UpdateBudgetDto Budget) : IRequest<BudgetDto>;