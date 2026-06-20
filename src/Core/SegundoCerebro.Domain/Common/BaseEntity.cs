namespace SegundoCerebro.Domain.Common;

/// <summary>
/// Clase base para todas las entidades del dominio, proporcionando propiedades comunes como Id, CreatedAt y UpdatedAt.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}