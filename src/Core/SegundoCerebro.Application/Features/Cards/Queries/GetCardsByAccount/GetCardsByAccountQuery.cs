using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Cards.Queries.GetCardsByAccount;

/// <summary>
/// Consulta para obtener todas las tarjetas asociadas a una cuenta específica.
/// </summary>
public record GetCardsByAccountQuery(Guid AccountId) : IRequest<IEnumerable<CardDto>>;
