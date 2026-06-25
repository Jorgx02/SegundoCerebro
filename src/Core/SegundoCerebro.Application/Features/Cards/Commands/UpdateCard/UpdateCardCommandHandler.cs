using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Exceptions;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Cards.Commands.UpdateCard;

/// <summary>
/// Handler para el comando UpdateCardCommand.
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

    /// <summary>
    /// Maneja la lógica para actualizar una tarjeta.
    /// </summary>
    /// <param name="request">El comando con los datos a actualizar.</param>
    /// <param name="cancellationToken">El token de cancelación.</param>
    /// <returns>Un DTO con la información de la tarjeta actualizada.</returns>
    /// <exception cref="NotFoundException">Se lanza si la tarjeta no se encuentra.</exception>
    public async Task<CardDto> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
    {
        var cardToUpdate = await _unitOfWork.Cards.GetByIdAsync(request.Id);
        if (cardToUpdate is null)
        {
            throw new NotFoundException(nameof(Card), request.Id);
        }

        _mapper.Map(request, cardToUpdate);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CardDto>(cardToUpdate);
    }
}