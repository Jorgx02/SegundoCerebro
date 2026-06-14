using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Projects.Queries.GetAllProjects;

public record GetAllProjectsQuery : IRequest<IEnumerable<ProjectDto>>;