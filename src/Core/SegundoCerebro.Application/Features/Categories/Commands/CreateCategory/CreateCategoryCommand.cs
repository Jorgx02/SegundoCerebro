using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Categories.Commands.CreateCategory;

/// <summary>
/// Comando para crear una nueva categoría de transacción.
/// </summary>
/// <param name="Category">DTO con los datos de la categoría a crear.</param>
public record CreateCategoryCommand(CreateCategoryDto Category) : IRequest<CategoryDto>;