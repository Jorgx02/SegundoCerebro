using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.TodoItems.Commands.UpdateTodoItem;

public record UpdateTodoItemCommand(Guid Id, UpdateTodoItemDto TodoItem) : IRequest<TodoItemDto>;