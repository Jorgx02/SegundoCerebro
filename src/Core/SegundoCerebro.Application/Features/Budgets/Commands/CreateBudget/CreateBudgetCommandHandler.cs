using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Budgets.Commands.CreateBudget;

/// <summary>
/// Manejador para el comando de creación de un presupuesto.
/// </summary>
public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, BudgetDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBudgetCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Procesa la solicitud de creación de un presupuesto.
    /// </summary>
    /// <param name="request">El comando con los datos del presupuesto.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El DTO del presupuesto recién creado.</returns>
    public async Task<BudgetDto> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
    {
        var budget = _mapper.Map<Budget>(request.Budget);
        budget.Id = Guid.NewGuid();
        budget.CreatedAt = DateTime.UtcNow;

        var createdBudget = await _unitOfWork.Budgets.AddAsync(budget);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<BudgetDto>(createdBudget);
    }
}