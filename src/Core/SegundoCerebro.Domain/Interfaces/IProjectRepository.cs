using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Domain.Interfaces;

/// <summary>
/// Define el contrato para el repositorio de Proyectos (Project), extendiendo el repositorio genérico.
/// Proporciona métodos de consulta específicos para la entidad Project.
/// </summary>
public interface IProjectRepository : IRepository<Project>
{
    /// <summary>
    /// Obtiene todos los proyectos activos.
    /// </summary>
    /// <returns>Una colección de los proyectos activos.</returns>
    Task<IEnumerable<Project>> GetActiveProjectsAsync();

    /// <summary>
    /// Obtiene un proyecto con sus detalles.
    /// </summary>
    /// <param name="id">El ID del proyecto.</param>
    /// <returns>El proyecto encontrado o null si no se encuentra.</returns>
    Task<Project?> GetWithTodoItemsAsync(Guid id);
}