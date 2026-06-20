using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Transactions.Queries.GetTransactionById;

/// <summary>
/// Consulta para obtener una transacción específica por su ID.
/// </summary>
/// <param name="Id">El identificador único de la transacción.</param>
public record GetTransactionByIdQuery(Guid Id) : IRequest<TransactionDto?>;