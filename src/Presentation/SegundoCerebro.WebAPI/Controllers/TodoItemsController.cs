using MediatR;
using Microsoft.AspNetCore.Mvc;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.TodoItems.Commands.CreateTodoItem;
using SegundoCerebro.Application.Features.TodoItems.Commands.DeleteTodoItem;
using SegundoCerebro.Application.Features.TodoItems.Commands.UpdateTodoItem;
using SegundoCerebro.Application.Features.TodoItems.Queries.GetTodoItemById;
using SegundoCerebro.Application.Features.TodoItems.Queries.GetAllTodoItems;

namespace SegundoCerebro.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
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
        var todoItems = await _mediator.Send(new GetAllTodoItemsQuery());
        return Ok(todoItems);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDto>> GetTodoItem(Guid id)
    {
        var todoItem = await _mediator.Send(new GetTodoItemByIdQuery(id));

        if (todoItem == null)
            return NotFound();

        return Ok(todoItem);
    }

    [HttpPost]
    public async Task<ActionResult<TodoItemDto>> CreateTodoItem(CreateTodoItemDto createTodoItemDto)
    {
        var todoItem = await _mediator.Send(new CreateTodoItemCommand(createTodoItemDto));
        return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TodoItemDto>> UpdateTodoItem(Guid id, UpdateTodoItemDto updateTodoItemDto)
    {
        try
        {
            var todoItem = await _mediator.Send(new UpdateTodoItemCommand(id, updateTodoItemDto));
            return Ok(todoItem);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTodoItem(Guid id)
    {
        var result = await _mediator.Send(new DeleteTodoItemCommand(id));

        if (!result)
            return NotFound();

        return NoContent();
    }
}