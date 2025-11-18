using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Transactions.Commands.UpdateTransaction;

public record UpdateTransactionCommand(Guid Id, UpdateTransactionDto Transaction) : IRequest<TransactionDto>;