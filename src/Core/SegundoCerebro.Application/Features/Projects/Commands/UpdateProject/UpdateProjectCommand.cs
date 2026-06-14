using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Projects.Commands.UpdateProject;

public record UpdateProjectCommand(Guid Id, UpdateProjectDto Project) : IRequest<ProjectDto>;
