using System.Linq.Expressions;

namespace SegundoCerebro.Domain.Interfaces;

/// <summary>
/// Define las operaciones base para un repositorio genérico (patrón Repositorio).
/// Proporciona una abstracción para el acceso a datos para una entidad <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">El tipo de la entidad para la que este repositorio opera.</typeparam>
public interface IGenericRepository<T> where T : class
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