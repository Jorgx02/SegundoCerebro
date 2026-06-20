using MediatR;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Budgets.Commands.DeleteBudget;

/// <summary>
/// Manejador para el comando de desactivación (Soft Delete) de un presupuesto.
/// </summary>
public class DeleteBudgetCommandHandler : IRequestHandler<DeleteBudgetCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBudgetCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Procesa la desactivación de un presupuesto, marcándolo como inactivo.
    /// </summary>
    /// <param name="request">El comando con el ID del presupuesto.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>True si la desactivación fue exitosa, False si el presupuesto no fue encontrado.</returns>
    public async Task<bool> Handle(DeleteBudgetCommand request, CancellationToken cancellationToken)
    {
        var budget = await _unitOfWork.Budgets.GetByIdAsync(request.Id);
        if (budget == null)
            return false;

        budget.IsActive = false;
        budget.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Budgets.UpdateAsync(budget);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}