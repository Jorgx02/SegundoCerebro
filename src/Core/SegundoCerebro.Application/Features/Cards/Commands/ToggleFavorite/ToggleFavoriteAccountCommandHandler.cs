using MediatR;
using SegundoCerebro.Application.Exceptions;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Accounts.Commands.ToggleFavorite;

/// <summary>
/// Handler para el comando ToggleFavoriteAccountCommand.
/// </summary>
public class ToggleFavoriteAccountCommandHandler : IRequestHandler<ToggleFavoriteAccountCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public ToggleFavoriteAccountCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ToggleFavoriteAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Accounts.GetByIdAsync(request.Id);
        if (account is null)
        {
            throw new NotFoundException(nameof(Account), request.Id);
        }

        account.IsFavorite = !account.IsFavorite;

        await _unitOfWork.SaveChangesAsync();
    }
}