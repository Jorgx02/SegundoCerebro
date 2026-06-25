using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Domain.Entities;

/// <summary>
/// Representa una cuenta financiera dentro del sistema (ej. Corriente, Ahorros).
/// Es la entidad principal del módulo financiero contra la cual se registran las transacciones.
/// </summary>
public class Account
{
    public Guid Id { get; set; }

    /// <summary>Nombre asignado por el usuario para identificar la cuenta (ej. "Nómina Santander").</summary>
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    /// <summary>Clasificación de la cuenta que determina sus reglas de negocio (ej. Checking, Savings).</summary>
    public AccountType Type { get; set; }

    /// <summary>Saldo actual de la cuenta. Se actualiza automáticamente mediante las transacciones.</summary>
    public decimal Balance { get; set; }

    /// <summary>Código ISO 4217 de la moneda (por defecto "EUR").</summary>
    public string Currency { get; set; } = "EUR";

    public string? BankName { get; set; }

    /// <summary>Número de cuenta oficial (ej. IBAN). Opcional, usado para integraciones o referencias reales.</summary>
    public string? AccountNumber { get; set; }

    /// <summary>Indica si la cuenta está operativa. Utiliza Soft-Delete (falso = archivada/borrada lógicamente).</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Indica si el usuario ha marcado esta cuenta como favorita para un acceso rápido.</summary>
    public bool IsFavorite { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Identificador del propietario de la cuenta (enlace con Identity).
    /// Garantiza el Multi-Tenancy y el aislamiento de datos.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    // Propiedades de navegación (Entity Framework Core)
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();

    /// <summary>
    /// Colección de tarjetas de crédito/débito asociadas a esta cuenta.
    /// </summary>
    public ICollection<Card> Cards { get; set; } = new List<Card>();
}