using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Domain.Interfaces;

public interface IProjectRepository : IRepository<Project>
{
    Task<IEnumerable<Project>> GetActiveProjectsAsync();
    Task<Project?> GetWithTodoItemsAsync(Guid id);
}