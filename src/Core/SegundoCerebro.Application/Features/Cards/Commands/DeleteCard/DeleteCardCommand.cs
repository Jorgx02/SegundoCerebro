using MediatR;

namespace SegundoCerebro.Application.Features.Cards.Commands.DeleteCard;

/// <summary>
/// Comando para eliminar una tarjeta específica por su ID.
/// </summary>
public record DeleteCardCommand(Guid Id) : IRequest<bool>;