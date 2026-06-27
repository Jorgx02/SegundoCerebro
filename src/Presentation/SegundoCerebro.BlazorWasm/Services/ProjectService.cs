using System.Net.Http.Json;
using SegundoCerebro.BlazorWasm.Models;
using SegundoCerebro.BlazorWasm.Models.Enums;

namespace SegundoCerebro.BlazorWasm.Services;

public class ProjectService : ApiService<ProjectDto, CreateProjectDto, UpdateProjectDto>, IProjectService
{
    public ProjectService(HttpClient httpClient) : base(httpClient, "projects")
    {
    }

    public async Task<IEnumerable<ProjectDto>> GetAllAsync(ProjectStatus? status = null)
    {
        var url = status.HasValue ? $"api/{_endpoint}?status={(int)status.Value}" : $"api/{_endpoint}";
        return await _httpClient.GetFromJsonAsync<IEnumerable<ProjectDto>>(url) ?? Enumerable.Empty<ProjectDto>();
    }
}