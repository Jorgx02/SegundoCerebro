using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Budgets.Commands.CreateBudget;

public record CreateBudgetCommand(CreateBudgetDto Budget) : IRequest<BudgetDto>;