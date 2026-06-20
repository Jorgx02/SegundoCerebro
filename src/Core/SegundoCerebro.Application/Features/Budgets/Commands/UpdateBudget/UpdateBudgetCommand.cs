using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Budgets.Commands.UpdateBudget;

/// <summary>
/// Comando para actualizar un presupuesto existente.
/// </summary>
/// <param name="Id">El identificador único del presupuesto a actualizar.</param>
/// <param name="Budget">DTO con los nuevos datos del presupuesto.</param>
public record UpdateBudgetCommand(Guid Id, UpdateBudgetDto Budget) : IRequest<BudgetDto>;