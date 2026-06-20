using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Categories.Queries.GetAllCategories;

/// <summary>
/// Manejador para la consulta que obtiene todas las categorías activas.
/// </summary>
public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Procesa la solicitud para obtener todas las categorías activas.
    /// </summary>
    /// <param name="request">La consulta.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Una colección de DTOs de las categorías activas.</returns>
    public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _unitOfWork.Categories.GetActiveCategoriesAsync();
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }
}