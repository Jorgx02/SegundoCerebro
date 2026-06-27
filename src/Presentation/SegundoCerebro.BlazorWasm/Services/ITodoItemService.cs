using SegundoCerebro.BlazorWasm.Models;
using SegundoCerebro.BlazorWasm.Models.Commands;

namespace SegundoCerebro.BlazorWasm.Services;

public interface ITodoItemService : IApiService<TodoItemDto, CreateTodoItemDto, UpdateTodoItemDto>
{
    Task<IEnumerable<TodoItemDto>> GetByProjectIdAsync(Guid projectId);
}