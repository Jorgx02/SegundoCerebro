using MediatR;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Transactions.Commands.DeleteTransaction;

/// <summary>
/// Manejador para el comando de eliminación de una transacción.
/// </summary>
public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTransactionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Procesa la eliminación de una transacción, revirtiendo su impacto en el saldo de la cuenta
    /// y en el gasto del presupuesto asociado, todo dentro de una transacción de base de datos.
    /// </summary>
    /// <param name="request">El comando con el ID de la transacción a eliminar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>True si la eliminación fue exitosa, False si la transacción no fue encontrada.</returns>
    /// <exception cref="Exception">Se relanza si ocurre un error durante la operación de base de datos.</exception>
    public async Task<bool> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(request.Id);
            if (transaction == null)
                return false;

            // 1. Revertir el balance de la cuenta
            var account = await _unitOfWork.Accounts.GetByIdAsync(transaction.AccountId);
            if (account != null)
            {
                if (transaction.Type == TransactionType.Income)
                    account.Balance -= transaction.Amount;
                else
                    account.Balance += transaction.Amount;

                account.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Accounts.UpdateAsync(account);
            }

            // 2. Revertir el gasto del presupuesto si aplica
            var budget = await _unitOfWork.Budgets.GetBudgetByCategoryAndPeriodAsync(
                transaction.CategoryId, transaction.Date);

            if (budget != null && transaction.Type == TransactionType.Expense)
            {
                await _unitOfWork.Budgets.UpdateBudgetSpentAsync(budget.Id, -transaction.Amount);
            }

            // 3. Eliminar la transacción
            await _unitOfWork.Transactions.DeleteAsync(transaction);

            // 4. Guardar y confirmar todos los cambios
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return true;
        }
        catch
        {
            // Si algo falla, revertir todos los cambios
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}