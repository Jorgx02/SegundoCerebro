using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Transactions.Queries.GetTransactionById;

/// <summary>
/// Manejador para la consulta que obtiene una transacción por su ID.
/// </summary>
public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, TransactionDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTransactionByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Procesa la solicitud para obtener una transacción por su ID.
    /// </summary>
    /// <param name="request">La consulta con el ID de la transacción.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El DTO de la transacción si se encuentra; de lo contrario, null.</returns>
    public async Task<TransactionDto?> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.Transactions.GetByIdAsync(request.Id);
        return transaction == null ? null : _mapper.Map<TransactionDto>(transaction);
    }
}