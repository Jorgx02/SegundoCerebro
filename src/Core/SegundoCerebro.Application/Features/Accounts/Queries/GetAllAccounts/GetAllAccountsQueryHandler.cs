using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SegundoCerebro.Application.Features.Accounts.Queries.GetAllAccounts;

/// <summary>
/// Manejador para la consulta GetAllAccountsQuery.
/// </summary>
public class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccountsQuery, IEnumerable<AccountDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllAccountsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Procesa la solicitud para obtener todas las cuentas del usuario, incluyendo el conteo de tarjetas asociadas.
    /// </summary>
    /// <param name="request">La consulta.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Una colección de DTOs de cuentas con el conteo de tarjetas.</returns>
    public async Task<IEnumerable<AccountDto>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _unitOfWork.Accounts.GetAllAsync();
        var allCards = await _unitOfWork.Cards.GetAllAsync();

        var cardCounts = allCards.GroupBy(c => c.AccountId).ToDictionary(g => g.Key, g => g.Count());

        var accountDtos = _mapper.Map<List<AccountDto>>(accounts);

        foreach (var dto in accountDtos)
        {
            dto.CardCount = cardCounts.GetValueOrDefault(dto.Id, 0);
        }

        return accountDtos;
    }
}