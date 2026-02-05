using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Domain.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>> GetByTypeAsync(Domain.Enums.CategoryType type);
    Task<IEnumerable<Category>> GetParentCategoriesAsync();
    Task<IEnumerable<Category>> GetSubCategoriesAsync(Guid parentCategoryId);
    Task<IEnumerable<Category>> GetActiveCategoriesAsync();
}