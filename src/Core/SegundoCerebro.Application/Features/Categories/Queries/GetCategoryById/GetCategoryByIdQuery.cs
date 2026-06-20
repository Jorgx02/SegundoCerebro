using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Categories.Queries.GetCategoryById;

/// <summary>
/// Consulta para obtener una categoría específica por su ID.
/// </summary>
/// <param name="Id">El identificador único de la categoría.</param>
public record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryDto?>;