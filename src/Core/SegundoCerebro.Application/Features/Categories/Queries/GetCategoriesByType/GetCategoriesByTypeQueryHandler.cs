using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Categories.Queries.GetCategoriesByType;

/// <summary>
/// Handler para la consulta GetCategoriesByTypeQuery.
/// </summary>
public class GetCategoriesByTypeQueryHandler : IRequestHandler<GetCategoriesByTypeQuery, IEnumerable<CategoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoriesByTypeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesByTypeQuery request, CancellationToken cancellationToken)
    {
        // Usamos un método específico del repositorio que filtra y ordena en la base de datos.
        var categories = await _unitOfWork.Categories.GetByTypeAsync(request.Type);
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }
}