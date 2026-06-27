using SegundoCerebro.BlazorWasm.Models;
using SegundoCerebro.BlazorWasm.Models.Enums;

namespace SegundoCerebro.BlazorWasm.Services;

public interface IProjectService : IApiService<ProjectDto, CreateProjectDto, UpdateProjectDto>
{
    Task<IEnumerable<ProjectDto>> GetAllAsync(ProjectStatus? status = null);
}