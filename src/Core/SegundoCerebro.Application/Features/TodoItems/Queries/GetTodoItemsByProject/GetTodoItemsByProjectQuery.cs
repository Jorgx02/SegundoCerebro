using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.TodoItems.Queries.GetTodoItemsByProject;

public record GetTodoItemsByProjectQuery(Guid ProjectId) : IRequest<IEnumerable<TodoItemDto>>;