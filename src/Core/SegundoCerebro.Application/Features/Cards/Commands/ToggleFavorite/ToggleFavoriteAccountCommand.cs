using MediatR;

namespace SegundoCerebro.Application.Features.Accounts.Commands.ToggleFavorite;

/// <summary>
/// Comando para cambiar el estado de 'favorito' de una cuenta.
/// </summary>
public record ToggleFavoriteAccountCommand(Guid Id) : IRequest;