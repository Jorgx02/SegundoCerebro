using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.TodoItems.Commands.CreateTodoItem;

public class CreateTodoItemCommand : IRequest<TodoItemDto>
{
    public CreateTodoItemDto TodoItemDto { get; set; } = null!;
}