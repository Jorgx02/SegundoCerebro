using SegundoCerebro.Domain.Common;
using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Domain.Entities;

/// <summary>
/// Representa una meta o límite de gasto para una categoría específica en un periodo de tiempo.
/// </summary>
public class Budget : BaseEntity
{
    /// <summary>Nombre identificativo del presupuesto (ej. "Presupuesto Ocio Verano").</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Descripción asociada al presupuesto.</summary>
    public string? Description { get; set; }

    /// <summary>Monto máximo que el usuario planea gastar en este presupuesto.</summary>
    public decimal Amount { get; set; }

    /// <summary>Periodicidad de reinicio del presupuesto (Mensual, Anual, etc.).</summary>
    public BudgetPeriod Period { get; set; }

    /// <summary>Fecha de inicio del presupuesto.</summary>
    public DateTime StartDate { get; set; }

    /// <summary>Fecha de finalización del presupuesto.</summary>
    public DateTime EndDate { get; set; }

    /// <summary>Indica si el presupuesto está actualmente en seguimiento o archivado.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Identificador del propietario (Aislamiento de datos / Multi-tenant).</summary>
    public string UserId { get; set; } = string.Empty;

    // Relaciones

    /// <summary>Categoría a la que se aplica el límite de gasto (Obligatorio).</summary>
    public Guid CategoryId { get; set; }


    public Category Category { get; set; } = null!;

    /// <summary>Cuenta específica opcional. Si es nulo, el presupuesto aplica a todas las cuentas del usuario.</summary>
    public Guid? AccountId { get; set; }
    public Account? Account { get; set; }

    /// <summary>
    /// Total acumulado gastado. Se actualiza automáticamente al añadir transacciones asociadas a este presupuesto.
    /// </summary>
    public decimal Spent { get; set; }
}