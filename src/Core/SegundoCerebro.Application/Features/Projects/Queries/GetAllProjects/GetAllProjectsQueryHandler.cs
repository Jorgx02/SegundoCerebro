using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Projects.Queries.GetAllProjects;

/// <summary>
/// Manejador para la consulta que obtiene todos los proyectos del usuario.
/// </summary>
public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, IEnumerable<ProjectDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllProjectsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Procesa la solicitud para obtener todos los proyectos.
    /// </summary>
    /// <param name="request">La consulta.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Una colección de DTOs de los proyectos.</returns>
    public async Task<IEnumerable<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await _unitOfWork.Projects.GetAllAsync();
        return _mapper.Map<IEnumerable<ProjectDto>>(projects);
    }
}