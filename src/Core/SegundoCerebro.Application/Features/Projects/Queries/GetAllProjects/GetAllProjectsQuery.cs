using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Application.Features.Projects.Queries.GetAllProjects;

public class GetAllProjectsQuery : IRequest<IEnumerable<ProjectDto>>
{
    public ProjectStatus? Status { get; set; }
}