using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.TodoItems.Queries.GetAllTodoItems;

/// <summary>
/// Consulta para obtener todas las tareas del usuario.
/// </summary>
public record GetAllTodoItemsQuery : IRequest<IEnumerable<TodoItemDto>>;