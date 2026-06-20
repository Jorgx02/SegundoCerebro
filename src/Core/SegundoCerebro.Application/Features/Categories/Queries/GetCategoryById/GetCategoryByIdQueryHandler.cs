using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Categories.Queries.GetCategoryById;

/// <summary>
/// Manejador para la consulta que obtiene una categoría por su ID.
/// </summary>
public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Procesa la solicitud para obtener una categoría por su ID.
    /// </summary>
    /// <param name="request">La consulta con el ID de la categoría.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El DTO de la categoría si se encuentra; de lo contrario, null.</returns>
    public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);
        return category == null ? null : _mapper.Map<CategoryDto>(category);
    }
}