using MediatR;
using SegundoCerebro.Application.Exceptions;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Cards.Commands.DeleteCard;

/// <summary>
/// Handler para el comando DeleteCardCommand.
/// </summary>
public class DeleteCardCommandHandler : IRequestHandler<DeleteCardCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCardCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Maneja la lógica para eliminar una tarjeta.
    /// </summary>
    /// <param name="request">El comando con el ID de la tarjeta a eliminar.</param>
    /// <param name="cancellationToken">El token de cancelación.</param>
    /// <returns>True si la tarjeta fue eliminada con éxito.</returns>
    /// <exception cref="NotFoundException">Se lanza si la tarjeta no se encuentra.</exception>
    public async Task<bool> Handle(DeleteCardCommand request, CancellationToken cancellationToken)
    {
        var card = await _unitOfWork.Cards.GetByIdAsync(request.Id);
        if (card is null)
        {
            throw new NotFoundException(nameof(Card), request.Id);
        }

        await _unitOfWork.Cards.DeleteAsync(card);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}