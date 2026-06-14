using Microsoft.EntityFrameworkCore;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Data;

namespace SegundoCerebro.Infrastructure.Repositories;

public class TodoItemRepository : Repository<TodoItem>, ITodoItemRepository
{
    public TodoItemRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<TodoItem>> GetByProjectIdAsync(Guid projectId)
    {
        return await _dbSet
            .Where(t => t.ProjectId == projectId)
            .OrderBy(t => t.DueDate ?? DateTime.MaxValue) // Las que no tienen fecha van al final
            .ToListAsync();
    }

    public async Task<IEnumerable<TodoItem>> GetByStatusAsync(TodoItemStatus status)
    {
        return await _dbSet
            .Include(t => t.Project)
            .Where(t => t.Status == status)
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.DueDate ?? DateTime.MaxValue)
            .ToListAsync();
    }

    public async Task<IEnumerable<TodoItem>> GetInboxItemsAsync()
    {
        return await _dbSet
            .Where(t => t.Status == TodoItemStatus.Inbox)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }
}