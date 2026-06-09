using MediatR;

namespace SegundoCerebro.Application.Features.Budgets.Commands.DeleteBudget;

public record DeleteBudgetCommand(Guid Id) : IRequest<bool>;