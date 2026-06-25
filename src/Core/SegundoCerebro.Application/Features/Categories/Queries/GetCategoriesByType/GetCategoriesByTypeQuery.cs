using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Application.Features.Categories.Queries.GetCategoriesByType;

/// <summary>
/// Consulta para obtener categorías filtradas por su tipo (Ingreso o Gasto).
/// </summary>
public record GetCategoriesByTypeQuery(CategoryType Type) : IRequest<IEnumerable<CategoryDto>>;