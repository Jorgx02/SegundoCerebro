using MediatR;
using SegundoCerebro.Application.DTOs;
using System.Text.Json.Serialization;

namespace SegundoCerebro.Application.Features.Cards.Commands.UpdateCard;

/// <summary>
/// Comando para actualizar los detalles de una tarjeta existente.
/// </summary>
public record UpdateCardCommand : IRequest<CardDto>
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Name { get; init; } = string.Empty;
    public int ExpirationMonth { get; init; }
    public int ExpirationYear { get; init; }
}
