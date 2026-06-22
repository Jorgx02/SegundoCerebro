namespace SegundoCerebro.Domain.Common;

/// <summary>
/// Clase base abstracta para entidades del dominio que comparten propiedades comunes.
/// Proporciona un identificador único (`Id`) y marcas de tiempo para auditoría (`CreatedAt`, `UpdatedAt`).
/// </summary>
public abstract class BaseEntity
{
    /// <summary>Identificador único de la entidad.</summary>
    public Guid Id { get; set; }

    /// <summary>Fecha y hora en UTC de cuando la entidad fue creada.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Fecha y hora en UTC de la última modificación de la entidad. Es nulo si nunca ha sido modificada.</summary>
    public DateTime? UpdatedAt { get; set; }
}