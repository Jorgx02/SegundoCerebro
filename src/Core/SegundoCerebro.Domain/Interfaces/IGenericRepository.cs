using System.Linq.Expressions;

namespace SegundoCerebro.Domain.Interfaces;

/// <summary>
/// Define el repositorio genérico.
/// Proporciona métodos de consulta específicos.
/// </summary>
public interface IGenericRespository<T> where T : class
{
    /// <summary>
    /// Obtiene todo.
    /// </summary>
    /// <returns>Una colección.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Obtiene algo dependiendo de su identificador.
    /// </summary>
    /// <param name="id">El tipo de algo a filtrar.</param>
    /// <returns>Una colección del tipo especificado.</returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Añade el elemento.
    /// </summary>
    /// <param name="entity">El elemento a añadir.</param>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Obtiene todas las categorías de un tipo específico.
    /// </summary>
    /// <param name="type">El tipo de categoría a filtrar.</param>
    /// <returns>Una colección de las categorías del tipo especificado.</returns>
    Task<IEnumerable<T>> GetByTypeAsync(Domain.Enums.CategoryType type);

    /// <summary>
    /// Actualiza el elemento.
    /// </summary>
    /// <param name="entity">El elemento a actualizar.</param>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Elimina el elemento.
    /// </summary>
    /// <param name="entity">El elemento a eliminar.</param>
    Task DeleteAsync(T entity);

    /// <summary>
    /// Encuentra elementos que coinciden con un predicado.
    /// </summary>
    /// <param name="predicate">La expresión lambda para filtrar los elementos.</param>
    /// <returns>Una colección de elementos que coinciden con el predicado.</returns>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
}