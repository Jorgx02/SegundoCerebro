using SegundoCerebro.BlazorWasm.Models;

namespace SegundoCerebro.BlazorWasm.Services;

public interface ITodoItemService : IApiService<TodoItemDto, CreateTodoItemDto, UpdateTodoItemDto>
{
    // En el futuro podemos añadir métodos como GetInboxItemsAsync()
}