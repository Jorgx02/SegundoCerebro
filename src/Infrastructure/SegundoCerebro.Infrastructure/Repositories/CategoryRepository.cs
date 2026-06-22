using Microsoft.EntityFrameworkCore;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Data;

namespace SegundoCerebro.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio para la entidad Categoría (`Category`).
/// </summary>
public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CategoryRepository"/>.
    /// </summary>
    /// <param name="context">El contexto de la base de datos.</param>
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtiene todas las categorías activas de un tipo específico (Ingreso o Gasto).
    /// </summary>
    public async Task<IEnumerable<Category>> GetByTypeAsync(Domain.Enums.CategoryType type)
    {
        return await _dbSet
            .Where(c => c.Type == type && c.IsActive)
            .Include(c => c.SubCategories)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene solo las categorías de nivel superior (aquellas que no tienen un padre).
    /// </summary>
    public async Task<IEnumerable<Category>> GetParentCategoriesAsync()
    {
        return await _dbSet
            .Where(c => c.ParentCategoryId == null && c.IsActive)
            .Include(c => c.SubCategories)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene todas las subcategorías directas de una categoría padre específica.
    /// </summary>
    public async Task<IEnumerable<Category>> GetSubCategoriesAsync(Guid parentCategoryId)
    {
        return await _dbSet
            .Where(c => c.ParentCategoryId == parentCategoryId && c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene todas las categorías que están marcadas como activas.
    /// </summary>
    public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
    {
        return await _dbSet
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Sobrescribe el método base para incluir siempre las categorías padre y subcategorías (Eager Loading),
    /// permitiendo construir el árbol de categorías completo en una sola consulta.
    /// </summary>
    public override async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _dbSet
            .Include(c => c.ParentCategory)
            .Include(c => c.SubCategories)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Sobrescribe el método base para incluir la categoría padre y las subcategorías (Eager Loading)
    /// al buscar una categoría específica por su ID.
    /// </summary>
    public override async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(c => c.ParentCategory)
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}