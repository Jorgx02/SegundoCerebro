using MediatR;
using SegundoCerebro.Application.DTOs;
using System.Text.Json.Serialization;

namespace SegundoCerebro.Application.Features.Cards.Commands.UpdateCard;

/// <summary>
/// Comando para actualizar el nombre de una tarjeta existente.
/// </summary>
public class UpdateCardCommand : IRequest<CardDto>
{
    /// <summary>
    /// ID de la tarjeta a actualizar. Se obtiene de la ruta de la API, no del cuerpo.
    /// </summary>
    [JsonIgnore]
    public Guid Id { get; set; }

    /// <summary>
    /// Nuevo nombre para la tarjeta.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}