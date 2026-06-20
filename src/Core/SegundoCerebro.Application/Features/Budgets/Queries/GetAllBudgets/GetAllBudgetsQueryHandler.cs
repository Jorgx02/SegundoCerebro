using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Budgets.Queries.GetAllBudgets;

/// <summary>
/// Manejador para la consulta que obtiene todos los presupuestos activos.
/// </summary>
public class GetAllBudgetsQueryHandler : IRequestHandler<GetAllBudgetsQuery, IEnumerable<BudgetDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllBudgetsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Procesa la solicitud para obtener todos los presupuestos activos.
    /// </summary>
    /// <param name="request">La consulta.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Una colección de DTOs de los presupuestos activos.</returns>
    public async Task<IEnumerable<BudgetDto>> Handle(GetAllBudgetsQuery request, CancellationToken cancellationToken)
    {
        var budgets = await _unitOfWork.Budgets.GetActiveBudgetsAsync();
        return _mapper.Map<IEnumerable<BudgetDto>>(budgets);
    }
}