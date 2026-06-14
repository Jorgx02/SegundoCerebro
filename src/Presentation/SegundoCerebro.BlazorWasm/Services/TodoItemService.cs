using SegundoCerebro.BlazorWasm.Models;

namespace SegundoCerebro.BlazorWasm.Services;

public class TodoItemService : ApiService<TodoItemDto, CreateTodoItemDto, UpdateTodoItemDto>, ITodoItemService
{
    public TodoItemService(HttpClient httpClient) : base(httpClient, "todoitems")
    {
    }
}