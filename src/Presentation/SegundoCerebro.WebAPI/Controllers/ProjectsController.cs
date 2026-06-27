using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Projects.Commands.CreateProject;
using SegundoCerebro.Application.Features.Projects.Commands.DeleteProject;
using SegundoCerebro.Application.Features.Projects.Commands.UpdateProject;
using SegundoCerebro.Application.Features.Projects.Queries.GetAllProjects;
using SegundoCerebro.Application.Features.Projects.Queries.GetProjectById;
using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.WebAPI.Controllers;

/// <summary>
/// Controlador para gestionar los proyectos de productividad.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los proyectos del usuario, con opción de filtrar por estado.
    /// </summary>
    /// <param name="status">Estado opcional para filtrar los proyectos.</param>
    /// <returns>Una colección de proyectos.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjects([FromQuery] ProjectStatus? status)
    {
        var query = new GetAllProjectsQuery { Status = status };
        var projects = await _mediator.Send(query);
        return Ok(projects);
    }

    /// <summary>
    /// Obtiene un proyecto específico por su ID.
    /// </summary>
    /// <param name="id">El ID del proyecto.</param>
    /// <returns>El proyecto solicitado.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetProject(Guid id)
    {
        var project = await _mediator.Send(new GetProjectByIdQuery(id));
        return Ok(project);
    }

    /// <summary>
    /// Crea un nuevo proyecto.
    /// </summary>
    /// <param name="projectDto">Los datos para el nuevo proyecto.</param>
    /// <returns>El proyecto recién creado.</returns>
    [HttpPost]
    public async Task<ActionResult<ProjectDto>> CreateProject(CreateProjectDto projectDto)
    {
        var command = new CreateProjectCommand { ProjectDto = projectDto };
        var project = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
    }

    /// <summary>
    /// Actualiza un proyecto existente.
    /// </summary>
    /// <param name="id">El ID del proyecto a actualizar.</param>
    /// <param name="projectDto">Los nuevos datos para el proyecto.</param>
    /// <returns>El proyecto actualizado.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ProjectDto>> UpdateProject(Guid id, UpdateProjectDto projectDto)
    {
        var command = new UpdateProjectCommand { Id = id, ProjectDto = projectDto };
        var project = await _mediator.Send(command);
        return Ok(project);
    }

    /// <summary>
    /// Elimina un proyecto. No se puede eliminar si está completado.
    /// </summary>
    /// <param name="id">El ID del proyecto a eliminar.</param>
    /// <returns>No devuelve contenido.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        await _mediator.Send(new DeleteProjectCommand(id));
        return NoContent();
    }
}