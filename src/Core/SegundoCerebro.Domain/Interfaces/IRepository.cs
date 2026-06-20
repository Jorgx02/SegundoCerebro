namespace SegundoCerebro.Domain.Interfaces;

/// <summary>
/// Define la interfaz para el repositorio genérico.
/// </summary>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Obtiene un elemento con sus detalles.
    /// </summary>
    /// <param name="id">El ID del elemento.</param>
    /// <returns>El elemento encontrado o null si no se encuentra.</returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Obtiene todos los elementos.
    /// </summary>
    /// <returns>Una colección de elementos.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Añade un nuevo elemento.
    /// </summary>
    /// <param name="entity">El elemento a añadir.</param>
    /// <returns>El elemento añadido.</returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Actualiza un elemento existente.
    /// </summary>
    /// <param name="entity">El elemento a actualizar.</param>
    /// <returns>El elemento actualizado.</returns>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Elimina un elemento.
    /// </summary>
    /// <param name="entity">El elemento a eliminar.</param>
    Task DeleteAsync(T entity);

    /// <summary>
    /// Verifica si un elemento existe.
    /// </summary>
    /// <param name="id">El ID del elemento.</param>
    /// <returns>True si el elemento existe, false en caso contrario.</returns>
    Task<bool> ExistsAsync(Guid id);
}