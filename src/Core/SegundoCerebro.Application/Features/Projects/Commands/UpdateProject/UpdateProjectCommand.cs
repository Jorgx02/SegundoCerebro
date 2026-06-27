using MediatR;
using SegundoCerebro.Application.DTOs;
using System.Text.Json.Serialization;

namespace SegundoCerebro.Application.Features.Projects.Commands.UpdateProject;

public class UpdateProjectCommand : IRequest<ProjectDto>
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public UpdateProjectDto ProjectDto { get; set; } = null!;
}