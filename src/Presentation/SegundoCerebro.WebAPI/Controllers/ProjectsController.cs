using MediatR;
using Microsoft.AspNetCore.Mvc;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Projects.Commands.CreateProject;
using SegundoCerebro.Application.Features.Projects.Commands.DeleteProject;
using SegundoCerebro.Application.Features.Projects.Commands.UpdateProject;
using SegundoCerebro.Application.Features.Projects.Queries.GetProjectById;
using SegundoCerebro.Application.Features.Projects.Queries.GetAllProjects;

namespace SegundoCerebro.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjects()
    {
        var projects = await _mediator.Send(new GetAllProjectsQuery());
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetProject(Guid id)
    {
        var project = await _mediator.Send(new GetProjectByIdQuery(id));

        if (project == null)
            return NotFound();

        return Ok(project);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectDto>> CreateProject(CreateProjectDto createProjectDto)
    {
        var project = await _mediator.Send(new CreateProjectCommand(createProjectDto));
        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProjectDto>> UpdateProject(Guid id, UpdateProjectDto updateProjectDto)
    {
        try
        {
            var project = await _mediator.Send(new UpdateProjectCommand(id, updateProjectDto));
            return Ok(project);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProject(Guid id)
    {
        var result = await _mediator.Send(new DeleteProjectCommand(id));

        if (!result)
            return NotFound();

        return NoContent();
    }
}