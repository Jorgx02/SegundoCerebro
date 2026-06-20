using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Categories.Commands.CreateCategory;

/// <summary>
/// Manejador para el comando de creación de una categoría.
/// </summary>
public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Procesa la solicitud de creación de una categoría.
    /// </summary>
    /// <param name="request">El comando con los datos de la categoría.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El DTO de la categoría recién creada.</returns>
    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = _mapper.Map<Category>(request.Category);
        category.Id = Guid.NewGuid();
        category.CreatedAt = DateTime.UtcNow;

        var createdCategory = await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CategoryDto>(createdCategory);
    }
}