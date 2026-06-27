using MediatR;
using SegundoCerebro.Application.DTOs;
using System.Text.Json.Serialization;

namespace SegundoCerebro.Application.Features.TodoItems.Commands.UpdateTodoItem;

public class UpdateTodoItemCommand : IRequest<TodoItemDto>
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public UpdateTodoItemDto TodoItemDto { get; set; } = null!;
}
