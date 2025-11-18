using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Transactions.Queries.GetTransactionById;

public record GetTransactionByIdQuery(Guid Id) : IRequest<TransactionDto?>;