using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Categories.Commands.UpdateCategory;

/// <summary>
/// Manejador para el comando de actualización de una categoría.
/// </summary>
public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Procesa la actualización de una categoría.
    /// </summary>
    /// <param name="request">El comando con el ID y los nuevos datos de la categoría.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El DTO de la categoría actualizada.</returns>
    public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var existingCategory = await _unitOfWork.Categories.GetByIdAsync(request.Id);
        if (existingCategory == null)
            throw new KeyNotFoundException($"Category with ID {request.Id} not found");

        _mapper.Map(request.Category, existingCategory);
        existingCategory.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Categories.UpdateAsync(existingCategory);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CategoryDto>(existingCategory);
    }
}