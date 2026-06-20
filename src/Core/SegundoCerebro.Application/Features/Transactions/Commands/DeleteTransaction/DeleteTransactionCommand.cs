using MediatR;

namespace SegundoCerebro.Application.Features.Transactions.Commands.DeleteTransaction;

/// <summary>
/// Comando para eliminar una transacción existente.
/// </summary>
/// <param name="Id">El identificador único de la transacción a eliminar.</param>
public record DeleteTransactionCommand(Guid Id) : IRequest<bool>;