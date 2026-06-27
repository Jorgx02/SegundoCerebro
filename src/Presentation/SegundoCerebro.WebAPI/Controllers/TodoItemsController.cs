using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.TodoItems.Commands.UpdateTodoItem;
using SegundoCerebro.Application.Features.TodoItems.Commands.DeleteTodoItem;
using SegundoCerebro.Application.Features.TodoItems.Commands.CreateTodoItem;
using SegundoCerebro.Application.Features.TodoItems.Queries.GetAllTodoItems;
using SegundoCerebro.Application.Features.TodoItems.Queries.GetTodoItemsByProject;

namespace SegundoCerebro.WebAPI.Controllers;

/// <summary>
/// Controlador para gestionar las tareas (TodoItems).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TodoItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TodoItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetAllTodoItems()
    {
        var query = new GetAllTodoItemsQuery();
        return Ok(await _mediator.Send(query));
    }

    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetByProject(Guid projectId)
    {
        var query = new GetTodoItemsByProjectQuery(projectId);
        var todoItems = await _mediator.Send(query);
        return Ok(todoItems);
    }

    [HttpPost]
    public async Task<ActionResult<TodoItemDto>> CreateTodoItem(CreateTodoItemDto todoItemDto)
    {
        var command = new CreateTodoItemCommand { TodoItemDto = todoItemDto };
        var todoItem = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetByProject), new { projectId = todoItem.ProjectId }, todoItem);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TodoItemDto>> UpdateTodoItem(Guid id, UpdateTodoItemDto todoItemDto)
    {
        var command = new UpdateTodoItemCommand { Id = id, TodoItemDto = todoItemDto };
        var todoItem = await _mediator.Send(command);
        return Ok(todoItem);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(Guid id)
    {
        var command = new DeleteTodoItemCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}