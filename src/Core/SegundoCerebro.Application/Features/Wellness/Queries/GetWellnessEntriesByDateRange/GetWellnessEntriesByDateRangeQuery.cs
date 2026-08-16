using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Wellness.Queries.GetWellnessEntriesByDateRange;

/// <summary>
/// Consulta para obtener las entradas del diario de bienestar en un rango de fechas.
/// </summary>
public record GetWellnessEntriesByDateRangeQuery(DateTime StartDate, DateTime EndDate) : IRequest<IEnumerable<WellnessEntryDto>>;