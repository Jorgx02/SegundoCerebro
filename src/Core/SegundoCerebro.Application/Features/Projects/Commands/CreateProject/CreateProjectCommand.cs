using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Projects.Commands.CreateProject;

public record CreateProjectCommand(CreateProjectDto Project) : IRequest<ProjectDto>;
