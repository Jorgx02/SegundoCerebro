using MediatR;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Accounts.Commands.DeleteAccount;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAccountCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Accounts.GetByIdAsync(request.Id);
        if (account == null)
            return false;

        // Soft delete
        account.IsActive = false;
        account.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Accounts.UpdateAsync(account);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}