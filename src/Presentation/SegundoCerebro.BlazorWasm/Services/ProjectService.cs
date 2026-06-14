using SegundoCerebro.BlazorWasm.Models;

namespace SegundoCerebro.BlazorWasm.Services;

public class ProjectService : ApiService<ProjectDto, CreateProjectDto, UpdateProjectDto>, IProjectService
{
    public ProjectService(HttpClient httpClient) : base(httpClient, "projects")
    {
    }
}