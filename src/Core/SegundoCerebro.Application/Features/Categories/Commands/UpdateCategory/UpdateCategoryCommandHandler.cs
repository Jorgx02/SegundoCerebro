using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

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