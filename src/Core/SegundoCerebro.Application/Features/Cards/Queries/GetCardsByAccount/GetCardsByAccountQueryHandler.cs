using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Exceptions;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Cards.Queries.GetCardsByAccount;

/// <summary>
/// Handler para la consulta GetCardsByAccountQuery.
/// </summary>
public class GetCardsByAccountQueryHandler : IRequestHandler<GetCardsByAccountQuery, IEnumerable<CardDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="GetCardsByAccountQueryHandler"/>.
    /// </summary>
    /// <param name="unitOfWork">La unidad de trabajo.</param>
    /// <param name="mapper">El mapeador de objetos.</param>
    public GetCardsByAccountQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Maneja la lógica para obtener las tarjetas de una cuenta.
    /// </summary>
    /// <param name="request">La consulta con el ID de la cuenta.</param>
    /// <param name="cancellationToken">El token de cancelación.</param>
    /// <returns>Una colección de DTOs de tarjetas.</returns>
    /// <exception cref="NotFoundException">Se lanza si la cuenta no existe.</exception>
    public async Task<IEnumerable<CardDto>> Handle(GetCardsByAccountQuery request, CancellationToken cancellationToken)
    {
        var accountExists = await _unitOfWork.Accounts.GetByIdAsync(request.AccountId);
        if (accountExists is null)
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }

        var cards = await _unitOfWork.Cards.GetByAccountIdAsync(request.AccountId);
        return _mapper.Map<IEnumerable<CardDto>>(cards);
    }
}
