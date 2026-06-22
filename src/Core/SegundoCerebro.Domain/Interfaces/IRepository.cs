namespace SegundoCerebro.Domain.Interfaces;

/// <summary>
/// Define un contrato genérico para los repositorios, encapsulando las operaciones CRUD estándar.
/// </summary>
/// <typeparam name="T">El tipo de la entidad que maneja el repositorio.</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Obtiene una entidad por su identificador único.
    /// </summary>
    /// <param name="id">El ID de la entidad a buscar.</param>
    /// <returns>La entidad encontrada, o null si no existe.</returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Obtiene todas las entidades del tipo <typeparamref name="T"/>.
    /// </summary>
    /// <returns>Una colección enumerable de todas las entidades.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Añade una nueva entidad al conjunto de datos.
    /// </summary>
    /// <param name="entity">La entidad a añadir.</param>
    /// <returns>La entidad añadida, después de ser procesada por el contexto.</returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Marca una entidad existente como modificada.
    /// </summary>
    /// <param name="entity">La entidad con los datos actualizados.</param>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Marca una entidad para ser eliminada.
    /// </summary>
    /// <param name="entity">La entidad a eliminar.</param>
    Task DeleteAsync(T entity);

    /// <summary>
    /// Comprueba de forma asíncrona si una entidad con el ID especificado existe.
    /// </summary>
    /// <param name="id">El ID de la entidad a comprobar.</param>
    /// <returns>Verdadero si la entidad existe, falso en caso contrario.</returns>
    Task<bool> ExistsAsync(Guid id);
}