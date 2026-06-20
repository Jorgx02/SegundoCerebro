using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Transactions.Queries.GetAllTransactions;

/// <summary>
/// Manejador para la consulta que obtiene todas las transacciones.
/// </summary>
public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, IEnumerable<TransactionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllTransactionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Procesa la solicitud para obtener todas las transacciones del usuario.
    /// </summary>
    /// <param name="request">La consulta.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Una colección de DTOs de transacciones.</returns>
    public async Task<IEnumerable<TransactionDto>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _unitOfWork.Transactions.GetAllAsync();
        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }
}