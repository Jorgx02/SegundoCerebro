using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Transactions.Commands.CreateTransaction;

public record CreateTransactionCommand(CreateTransactionDto Transaction) : IRequest<TransactionDto>;