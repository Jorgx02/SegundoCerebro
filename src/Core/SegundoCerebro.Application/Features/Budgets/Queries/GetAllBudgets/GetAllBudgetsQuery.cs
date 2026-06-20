using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Budgets.Queries.GetAllBudgets;

/// <summary>
/// Consulta para obtener todos los presupuestos activos del usuario.
/// </summary>
public record GetAllBudgetsQuery : IRequest<IEnumerable<BudgetDto>>;