using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.TodoItems.Commands.CreateTodoItem;

public record CreateTodoItemCommand(CreateTodoItemDto TodoItem) : IRequest<TodoItemDto>;