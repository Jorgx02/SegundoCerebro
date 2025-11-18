using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(CreateCategoryDto Category) : IRequest<CategoryDto>;