using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Application.DTOs;

/// <summary>
/// DTO para representar los datos de una cuenta financiera al ser consultada.
/// </summary>
public class AccountDto
{
    /// <summary>Identificador único de la cuenta.</summary>
    public Guid Id { get; set; }
    /// <summary>Nombre asignado por el usuario a la cuenta.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Descripción opcional de la cuenta.</summary>
    public string? Description { get; set; }
    /// <summary>Tipo de cuenta (ej. Corriente, Ahorro).</summary>
    public AccountType Type { get; set; }
    /// <summary>Nombre legible del tipo de cuenta.</summary>
    public string TypeName => Type.ToString();
    /// <summary>Saldo actual de la cuenta.</summary>
    public decimal Balance { get; set; }
    /// <summary>Código de la moneda (ej. "EUR").</summary>
    public string Currency { get; set; } = "EUR";
    /// <summary>Nombre del banco asociado.</summary>
    public string? BankName { get; set; }
    /// <summary>Número de cuenta (ej. IBAN).</summary>
    public string? AccountNumber { get; set; }
    /// <summary>Indica si la cuenta está activa o archivada (Soft Delete).</summary>
    public bool IsActive { get; set; }
    /// <summary>Indica si el usuario ha marcado esta cuenta como favorita.</summary>
    public bool IsFavorite { get; set; }
    /// <summary>Fecha de creación de la entidad.</summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>Fecha de la última actualización.</summary>
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// DTO utilizado para crear una nueva cuenta financiera.
/// </summary>
public class CreateAccountDto
{
    /// <summary>Nombre de la nueva cuenta.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Descripción opcional.</summary>
    public string? Description { get; set; }
    /// <summary>Tipo de la nueva cuenta.</summary>
    public AccountType Type { get; set; }
    /// <summary>Saldo con el que se creará la cuenta.</summary>
    public decimal InitialBalance { get; set; }
    /// <summary>Código de la moneda.</summary>
    public string Currency { get; set; } = "EUR";
    /// <summary>Nombre del banco.</summary>
    public string? BankName { get; set; }
    /// <summary>Número de cuenta.</summary>
    public string? AccountNumber { get; set; }
}

/// <summary>
/// DTO utilizado para actualizar una cuenta financiera existente.
/// </summary>
public class UpdateAccountDto
{
    /// <summary>Nuevo nombre para la cuenta.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Nueva descripción opcional.</summary>
    public string? Description { get; set; }
    /// <summary>Nuevo tipo de cuenta.</summary>
    public AccountType Type { get; set; }
    /// <summary>Nuevo código de moneda.</summary>
    public string Currency { get; set; } = "EUR";
    /// <summary>Nuevo nombre de banco.</summary>
    public string? BankName { get; set; }
    /// <summary>Nuevo número de cuenta.</summary>
    public string? AccountNumber { get; set; }
    /// <summary>Nuevo estado de activación.</summary>
    public bool IsActive { get; set; }
    /// <summary>Nuevo saldo de la cuenta.</summary>
    public decimal Balance { get; set; }
}