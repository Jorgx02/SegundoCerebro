using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Transactions.Commands.UpdateTransaction;

public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, TransactionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTransactionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TransactionDto> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var existingTransaction = await _unitOfWork.Transactions.GetByIdAsync(request.Id);
            if (existingTransaction == null)
                throw new KeyNotFoundException($"Transaction with ID {request.Id} not found");

            // Revert previous account balance
            var account = await _unitOfWork.Accounts.GetByIdAsync(existingTransaction.AccountId);
            if (account != null)
            {
                if (existingTransaction.Type == TransactionType.Income)
                    account.Balance -= existingTransaction.Amount;
                else
                    account.Balance += existingTransaction.Amount;
            }

            // Update transaction
            _mapper.Map(request.Transaction, existingTransaction);
            existingTransaction.UpdatedAt = DateTime.UtcNow;

            // Apply new account balance
            var newAccount = await _unitOfWork.Accounts.GetByIdAsync(existingTransaction.AccountId);
            if (newAccount != null)
            {
                if (existingTransaction.Type == TransactionType.Income)
                    newAccount.Balance += existingTransaction.Amount;
                else
                    newAccount.Balance -= existingTransaction.Amount;

                newAccount.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Accounts.UpdateAsync(newAccount);
            }

            await _unitOfWork.Transactions.UpdateAsync(existingTransaction);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return _mapper.Map<TransactionDto>(existingTransaction);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}