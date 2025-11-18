using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

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