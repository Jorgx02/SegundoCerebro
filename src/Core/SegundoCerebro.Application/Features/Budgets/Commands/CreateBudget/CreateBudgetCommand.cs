using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Budgets.Commands.CreateBudget;

/// <summary>
/// Comando para crear un nuevo presupuesto.
/// </summary>
/// <param name="Budget">DTO con los datos del presupuesto a crear.</param>
public record CreateBudgetCommand(CreateBudgetDto Budget) : IRequest<BudgetDto>;