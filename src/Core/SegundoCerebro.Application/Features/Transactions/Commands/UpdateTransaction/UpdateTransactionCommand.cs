using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Transactions.Commands.UpdateTransaction;

/// <summary>
/// Comando para actualizar una transacción existente.
/// </summary>
/// <param name="Id">El identificador único de la transacción a actualizar.</param>
/// <param name="Transaction">DTO con los nuevos datos de la transacción.</param>
public record UpdateTransactionCommand(Guid Id, UpdateTransactionDto Transaction) : IRequest<TransactionDto>;