using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Transactions.Queries.GetAllTransactions;

/// <summary>
/// Consulta para obtener todas las transacciones del usuario.
/// </summary>
public record GetAllTransactionsQuery : IRequest<IEnumerable<TransactionDto>>;