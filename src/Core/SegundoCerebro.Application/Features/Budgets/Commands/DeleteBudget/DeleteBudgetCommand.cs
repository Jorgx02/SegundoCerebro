using MediatR;

namespace SegundoCerebro.Application.Features.Budgets.Commands.DeleteBudget;

/// <summary>
/// Comando para desactivar (Soft Delete) un presupuesto existente.
/// </summary>
/// <param name="Id">El identificador único del presupuesto a desactivar.</param>
public record DeleteBudgetCommand(Guid Id) : IRequest<bool>;