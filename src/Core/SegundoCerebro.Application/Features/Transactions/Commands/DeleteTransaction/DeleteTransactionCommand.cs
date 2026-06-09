using MediatR;

namespace SegundoCerebro.Application.Features.Transactions.Commands.DeleteTransaction;

public record DeleteTransactionCommand(Guid Id) : IRequest<bool>;