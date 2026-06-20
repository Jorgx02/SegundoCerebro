using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Domain.Entities;

/// <summary>
/// Representa un movimiento de dinero (ingreso o gasto) asociado a una cuenta específica.
/// </summary>
public class Transaction
{
    public Guid Id { get; set; }

    /// <summary>Breve descripción del movimiento (ej. "Compra Mercadona").</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Valor absoluto del movimiento. El signo lógico lo determina la propiedad Type.</summary>
    public decimal Amount { get; set; }

    /// <summary>Determina si es un ingreso (Income) o un gasto (Expense).</summary>
    public TransactionType Type { get; set; }

    /// <summary>Fecha en la que ocurrió la transacción financieramente.</summary>
    public DateTime Date { get; set; }

    /// <summary>Identificador opcional del ticket, factura o referencia bancaria externa.</summary>
    public string? Reference { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    /// <summary>Identificador del propietario (Aislamiento de datos / Multi-tenant).</summary>
    public string UserId { get; set; } = string.Empty;

    // Claves Foráneas
    public Guid AccountId { get; set; }
    public Guid CategoryId { get; set; }

    // Propiedades de Navegación
    public Account Account { get; set; } = null!;
    public Category Category { get; set; } = null!;
}