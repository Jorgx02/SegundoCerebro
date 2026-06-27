using System.Net.Http.Json;
using SegundoCerebro.BlazorWasm.Models;
using SegundoCerebro.BlazorWasm.Models.Commands;

namespace SegundoCerebro.BlazorWasm.Services;

public class TodoItemService : ApiService<TodoItemDto, CreateTodoItemDto, UpdateTodoItemDto>, ITodoItemService
{
    public TodoItemService(HttpClient httpClient) : base(httpClient, "todoitems")
    {
    }

    public async Task<IEnumerable<TodoItemDto>> GetByProjectIdAsync(Guid projectId)
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<TodoItemDto>>($"api/todoitems/project/{projectId}") ?? Enumerable.Empty<TodoItemDto>();
    }
}