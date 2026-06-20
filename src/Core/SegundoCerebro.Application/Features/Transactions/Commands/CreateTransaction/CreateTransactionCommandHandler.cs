using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Transactions.Commands.CreateTransaction;

/// <summary>
/// Manejador para el comando de creación de una transacción.
/// </summary>
public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, TransactionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTransactionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Procesa la creación de una nueva transacción, actualizando el saldo de la cuenta
    /// y el gasto del presupuesto correspondiente en una única operación transaccional.
    /// </summary>
    /// <param name="request">El comando con los datos de la transacción.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El DTO de la transacción creada.</returns>
    /// <exception cref="KeyNotFoundException">Se lanza si la cuenta asociada no existe.</exception>
    public async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var transaction = _mapper.Map<Transaction>(request.Transaction);
            transaction.Id = Guid.NewGuid();
            transaction.CreatedAt = DateTime.UtcNow;

            // 1. Actualizar el saldo de la cuenta
            var account = await _unitOfWork.Accounts.GetByIdAsync(transaction.AccountId);
            if (account == null)
                throw new KeyNotFoundException($"Account with ID {transaction.AccountId} not found");

            if (transaction.Type == TransactionType.Income)
                account.Balance += transaction.Amount;
            else
                account.Balance -= transaction.Amount;

            account.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Accounts.UpdateAsync(account);

            // 2. Actualizar el presupuesto si existe uno activo para la categoría y fecha
            var budget = await _unitOfWork.Budgets.GetBudgetByCategoryAndPeriodAsync(
                transaction.CategoryId, transaction.Date);

            if (budget != null && transaction.Type == TransactionType.Expense)
            {
                await _unitOfWork.Budgets.UpdateBudgetSpentAsync(budget.Id, transaction.Amount);
            }

            // 3. Añadir la transacción
            var createdTransaction = await _unitOfWork.Transactions.AddAsync(transaction);

            // 4. Guardar todos los cambios y confirmar la transacción
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return _mapper.Map<TransactionDto>(createdTransaction);
        }
        catch
        {
            // Si algo falla, revertir todos los cambios en la base de datos
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}