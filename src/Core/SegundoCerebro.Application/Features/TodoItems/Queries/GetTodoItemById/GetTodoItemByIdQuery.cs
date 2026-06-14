using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.TodoItems.Queries.GetTodoItemById;

public record GetTodoItemByIdQuery(Guid Id) : IRequest<TodoItemDto?>;