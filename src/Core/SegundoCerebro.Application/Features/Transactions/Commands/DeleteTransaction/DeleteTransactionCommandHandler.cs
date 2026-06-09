using MediatR;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Transactions.Commands.DeleteTransaction;

public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTransactionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(request.Id);
            if (transaction == null)
                return false;

            // Revertir el balance de la cuenta
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

            // Revertir el gasto del presupuesto si aplica
            var budget = await _unitOfWork.Budgets.GetBudgetByCategoryAndPeriodAsync(
                transaction.CategoryId, transaction.Date);

            if (budget != null && transaction.Type == TransactionType.Expense)
            {
                await _unitOfWork.Budgets.UpdateBudgetSpentAsync(budget.Id, -transaction.Amount);
            }

            await _unitOfWork.Transactions.DeleteAsync(transaction);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return true;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}