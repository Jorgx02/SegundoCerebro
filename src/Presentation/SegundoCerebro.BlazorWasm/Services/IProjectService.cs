using SegundoCerebro.BlazorWasm.Models;

namespace SegundoCerebro.BlazorWasm.Services;

public interface IProjectService : IApiService<ProjectDto, CreateProjectDto, UpdateProjectDto>
{
    // Aquí podemos añadir en el futuro métodos específicos como GetProjectsWithTodoItemsAsync()
}