using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Cards.Queries.GetCardById;

/// <summary>
/// Consulta para obtener una tarjeta específica por su ID.
/// </summary>
public record GetCardByIdQuery(Guid Id) : IRequest<CardDto>;
