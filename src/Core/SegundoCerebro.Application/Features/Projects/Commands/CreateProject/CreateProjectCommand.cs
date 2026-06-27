using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Projects.Commands.CreateProject;

public class CreateProjectCommand : IRequest<ProjectDto>
{
    public CreateProjectDto ProjectDto { get; set; } = null!;
}