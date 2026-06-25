namespace SegundoCerebro.Application.Exceptions;

/// <summary>
/// Excepción que se lanza cuando no se encuentra una entidad específica.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="NotFoundException"/>.
    /// </summary>
    /// <param name="name">El nombre de la entidad.</param>
    /// <param name="key">La clave o ID de la entidad que no se encontró.</param>
    public NotFoundException(string name, object key)
        : base($"La entidad \"{name}\" con la clave ({key}) no fue encontrada.")
    {
    }
}