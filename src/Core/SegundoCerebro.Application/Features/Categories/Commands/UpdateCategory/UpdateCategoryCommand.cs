using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(Guid Id, UpdateCategoryDto Category) : IRequest<CategoryDto>;