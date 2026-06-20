using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Domain.Interfaces;

/// <summary>
/// Define el contrato para el repositorio de Categorías (Category), extendiendo el repositorio genérico.
/// Proporciona métodos de consulta específicos para la entidad Category.
/// </summary>
public interface ICategoryRepository : IRepository<Category>
{
    /// <summary>
    /// Obtiene todas las categorías de un tipo específico.
    /// </summary>
    /// <param name="type">El tipo de categoría a filtrar.</param>
    /// <returns>Una colección de las categorías del tipo especificado.</returns>
    Task<IEnumerable<Category>> GetByTypeAsync(Domain.Enums.CategoryType type);

    /// <summary>
    /// Obtiene todas las categorías padre.
    /// </summary>
    /// <returns>Una colección de las categorías padre.</returns>
    Task<IEnumerable<Category>> GetParentCategoriesAsync();

    /// <summary>
    /// Obtiene todas las categorías secundarias de una categoría padre.
    /// </summary>
    /// <param name="parentCategoryId">El ID de la categoría padre.</param>
    /// <returns>Una colección de las categorías secundarias.</returns>
    Task<IEnumerable<Category>> GetSubCategoriesAsync(Guid parentCategoryId);

    /// <summary>
    /// Obtiene todas las categorías activas.
    /// </summary>
    /// <returns>Una colección de las categorías activas.</returns>
    Task<IEnumerable<Category>> GetActiveCategoriesAsync();
}