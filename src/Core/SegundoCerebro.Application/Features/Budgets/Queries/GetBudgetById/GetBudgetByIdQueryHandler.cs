using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Budgets.Queries.GetBudgetById;

/// <summary>
/// Manejador para la consulta que obtiene un presupuesto por su ID.
/// </summary>
public class GetBudgetByIdQueryHandler : IRequestHandler<GetBudgetByIdQuery, BudgetDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBudgetByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Procesa la solicitud para obtener un presupuesto por su ID.
    /// </summary>
    /// <param name="request">La consulta con el ID del presupuesto.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El DTO del presupuesto si se encuentra; de lo contrario, null.</returns>
    public async Task<BudgetDto?> Handle(GetBudgetByIdQuery request, CancellationToken cancellationToken)
    {
        var budget = await _unitOfWork.Budgets.GetByIdAsync(request.Id);
        return budget == null ? null : _mapper.Map<BudgetDto>(budget);
    }
}