using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Accounts.Queries.GetAllAccounts;

/// <summary>
/// Manejador para la consulta que obtiene todas las cuentas activas.
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
    /// Procesa la solicitud para obtener todas las cuentas activas.
    /// </summary>
    /// <param name="request">La consulta.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Una colección de DTOs de las cuentas activas.</returns>
    public async Task<IEnumerable<AccountDto>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _unitOfWork.Accounts.GetActiveAccountsAsync();
        return _mapper.Map<IEnumerable<AccountDto>>(accounts);
    }
}