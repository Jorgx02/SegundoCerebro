using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Categories.Queries.GetAllCategories;

/// <summary>
/// Consulta para obtener todas las categorías activas.
/// </summary>
public record GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>;