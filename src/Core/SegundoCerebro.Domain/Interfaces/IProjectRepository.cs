using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Domain.Interfaces;

public interface IProjectRepository : IRepository<Project>
{
    Task<Project?> GetWithTodoItemsAsync(Guid projectId);
}