using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Transactions.Queries.GetAllTransactions;

public record GetAllTransactionsQuery : IRequest<IEnumerable<TransactionDto>>;