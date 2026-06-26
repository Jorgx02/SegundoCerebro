using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Cards.Commands.CreateCard;

/// <summary>
/// Comando para crear una nueva tarjeta y asociarla a una cuenta.
/// </summary>
public record CreateCardCommand : IRequest<CardDto>
{
    public Guid AccountId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string StripePaymentMethodId { get; init; } = string.Empty;
}