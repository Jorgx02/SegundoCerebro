using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Budgets.Commands.UpdateBudget;

/// <summary>
/// Manejador para el comando de actualización de un presupuesto.
/// </summary>
public class UpdateBudgetCommandHandler : IRequestHandler<UpdateBudgetCommand, BudgetDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateBudgetCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Procesa la actualización de un presupuesto.
    /// </summary>
    /// <param name="request">El comando con el ID y los nuevos datos del presupuesto.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El DTO del presupuesto actualizado.</returns>
    public async Task<BudgetDto> Handle(UpdateBudgetCommand request, CancellationToken cancellationToken)
    {
        var existingBudget = await _unitOfWork.Budgets.GetByIdAsync(request.Id);
        if (existingBudget == null)
            throw new KeyNotFoundException($"Budget with ID {request.Id} not found");

        _mapper.Map(request.Budget, existingBudget);
        existingBudget.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Budgets.UpdateAsync(existingBudget);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<BudgetDto>(existingBudget);
    }
}