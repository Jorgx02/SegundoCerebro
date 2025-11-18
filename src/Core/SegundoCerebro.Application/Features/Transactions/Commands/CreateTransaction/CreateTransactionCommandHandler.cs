using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, TransactionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTransactionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var transaction = _mapper.Map<Transaction>(request.Transaction);
            transaction.Id = Guid.NewGuid();
            transaction.CreatedAt = DateTime.UtcNow;

            // Update account balance
            var account = await _unitOfWork.Accounts.GetByIdAsync(transaction.AccountId);
            if (account == null)
                throw new KeyNotFoundException($"Account with ID {transaction.AccountId} not found");

            if (transaction.Type == TransactionType.Income)
                account.Balance += transaction.Amount;
            else
                account.Balance -= transaction.Amount;

            account.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Accounts.UpdateAsync(account);

            // Update budget if exists
            var budget = await _unitOfWork.Budgets.GetBudgetByCategoryAndPeriodAsync(
                transaction.CategoryId, transaction.Date);
            
            if (budget != null && transaction.Type == TransactionType.Expense)
            {
                await _unitOfWork.Budgets.UpdateBudgetSpentAsync(budget.Id, transaction.Amount);
            }

            var createdTransaction = await _unitOfWork.Transactions.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return _mapper.Map<TransactionDto>(createdTransaction);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}