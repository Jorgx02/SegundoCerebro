using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Domain.Interfaces;

public interface ITodoItemRepository : IRepository<TodoItem>
{
    Task<IEnumerable<TodoItem>> GetByProjectIdAsync(Guid projectId);
    Task<IEnumerable<TodoItem>> GetByStatusAsync(TodoItemStatus status);
    Task<IEnumerable<TodoItem>> GetInboxItemsAsync();
    Task<IEnumerable<TodoItem>> GetTodoItemsWithProjectsAsync();
}