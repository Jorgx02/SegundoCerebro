using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Budgets.Queries.GetBudgetById;

/// <summary>
/// Consulta para obtener un presupuesto específico por su ID.
/// </summary>
/// <param name="Id">El identificador único del presupuesto.</param>
public record GetBudgetByIdQuery(Guid Id) : IRequest<BudgetDto?>;