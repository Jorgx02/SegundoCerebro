using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Application.DTOs;

/// <summary>
/// DTO para representar los datos de una transacción al ser consultada.
/// </summary>
public class TransactionDto
{
    /// <summary>Identificador único de la transacción.</summary>
    public Guid Id { get; set; }
    /// <summary>Descripción del movimiento.</summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>Monto de la transacción (siempre positivo).</summary>
    public decimal Amount { get; set; }
    /// <summary>Tipo de transacción (Ingreso o Gasto).</summary>
    public TransactionType Type { get; set; }
    /// <summary>Nombre legible del tipo de transacción.</summary>
    public string TypeName => Type.ToString();
    /// <summary>Fecha en que ocurrió la transacción.</summary>
    public DateTime Date { get; set; }
    /// <summary>Referencia externa opcional (ej. número de factura).</summary>
    public string? Reference { get; set; }
    /// <summary>Notas adicionales sobre la transacción.</summary>
    public string? Notes { get; set; }
    /// <summary>Fecha de creación del registro.</summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>Fecha de la última actualización.</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>ID de la cuenta a la que pertenece la transacción.</summary>
    public Guid AccountId { get; set; }
    /// <summary>Nombre de la cuenta asociada.</summary>
    public string AccountName { get; set; } = string.Empty;

    /// <summary>ID de la categoría de la transacción.</summary>
    public Guid CategoryId { get; set; }
    /// <summary>Nombre de la categoría asociada.</summary>
    public string CategoryName { get; set; } = string.Empty;
    /// <summary>Icono de la categoría asociada.</summary>
    public string? CategoryIcon { get; set; }
    /// <summary>Color de la categoría asociada.</summary>
    public string? CategoryColor { get; set; }

    /// <summary>Propiedad calculada que formatea el monto con signo y símbolo de moneda.</summary>
    public string FormattedAmount => Type == TransactionType.Income
        ? $"+{Amount:C}"
        : $"-{Amount:C}";
}

/// <summary>
/// DTO utilizado para crear una nueva transacción.
/// </summary>
public class CreateTransactionDto
{
    /// <summary>Descripción del movimiento.</summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>Monto de la transacción.</summary>
    public decimal Amount { get; set; }
    /// <summary>Tipo (Ingreso/Gasto).</summary>
    public TransactionType Type { get; set; }
    /// <summary>Fecha de la transacción.</summary>
    public DateTime Date { get; set; } = DateTime.Now;
    /// <summary>Referencia externa opcional.</summary>
    public string? Reference { get; set; }
    /// <summary>Notas adicionales.</summary>
    public string? Notes { get; set; }
    /// <summary>ID de la cuenta afectada.</summary>
    public Guid AccountId { get; set; }
    /// <summary>ID de la categoría a la que pertenece.</summary>
    public Guid CategoryId { get; set; }
}

/// <summary>
/// DTO utilizado para actualizar una transacción existente.
/// </summary>
public class UpdateTransactionDto
{
    /// <summary>Nueva descripción.</summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>Nuevo monto.</summary>
    public decimal Amount { get; set; }
    /// <summary>Nuevo tipo.</summary>
    public TransactionType Type { get; set; }
    /// <summary>Nueva fecha.</summary>
    public DateTime Date { get; set; }
    /// <summary>Nueva referencia opcional.</summary>
    public string? Reference { get; set; }
    /// <summary>Nuevas notas adicionales.</summary>
    public string? Notes { get; set; }
    /// <summary>ID de la cuenta (generalmente no se modifica, pero se incluye por completitud).</summary>
    public Guid AccountId { get; set; }
    /// <summary>Nuevo ID de la categoría.</summary>
    public Guid CategoryId { get; set; }
}