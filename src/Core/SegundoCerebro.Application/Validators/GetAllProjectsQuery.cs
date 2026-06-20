using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Projects.Queries.GetAllProjects;

/// <summary>
/// Consulta para obtener todos los proyectos del usuario.
/// </summary>
public record GetAllProjectsQuery : IRequest<IEnumerable<ProjectDto>>;