using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Categories.Commands.UpdateCategory;

/// <summary>
/// Comando para actualizar una categoría existente.
/// </summary>
/// <param name="Id">El identificador único de la categoría a actualizar.</param>
/// <param name="Category">DTO con los nuevos datos de la categoría.</param>
public record UpdateCategoryCommand(Guid Id, UpdateCategoryDto Category) : IRequest<CategoryDto>;