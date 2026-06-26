using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Exceptions;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Cards.Commands.UpdateCard;

/// <summary>
/// Manejador para el comando de actualización de una tarjeta.
/// </summary>
public class UpdateCardCommandHandler : IRequestHandler<UpdateCardCommand, CardDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCardCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CardDto> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
    {
        var cardToUpdate = await _unitOfWork.Cards.GetByIdAsync(request.Id);
        if (cardToUpdate is null)
        {
            throw new NotFoundException(nameof(Card), request.Id);
        }

        cardToUpdate.Name = request.Name;
        cardToUpdate.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CardDto>(cardToUpdate);
    }
}