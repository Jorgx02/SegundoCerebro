using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Exceptions;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Cards.Queries.GetCardById;

/// <summary>
/// Handler para la consulta GetCardByIdQuery.
/// </summary>
public class GetCardByIdQueryHandler : IRequestHandler<GetCardByIdQuery, CardDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCardByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Maneja la lógica para obtener una tarjeta por su ID.
    /// </summary>
    /// <param name="request">La consulta con el ID de la tarjeta.</param>
    /// <param name="cancellationToken">El token de cancelación.</param>
    /// <returns>Un DTO con la información de la tarjeta.</returns>
    /// <exception cref="NotFoundException">Se lanza si la tarjeta no se encuentra.</exception>
    public async Task<CardDto> Handle(GetCardByIdQuery request, CancellationToken cancellationToken)
    {
        var card = await _unitOfWork.Cards.GetByIdAsync(request.Id);
        if (card is null)
        {
            throw new NotFoundException(nameof(Card), request.Id);
        }
        return _mapper.Map<CardDto>(card);
    }
}

