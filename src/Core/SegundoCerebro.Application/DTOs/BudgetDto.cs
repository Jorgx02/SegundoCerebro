using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Application.DTOs;

/// <summary>
/// DTO para representar los datos de un presupuesto al ser consultado.
/// </summary>
public class BudgetDto
{
    /// <summary>Identificador único del presupuesto.</summary>
    public Guid Id { get; set; }
    /// <summary>Nombre del presupuesto.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Descripción opcional.</summary>
    public string? Description { get; set; }
    /// <summary>Monto total asignado al presupuesto.</summary>
    public decimal Amount { get; set; }
    /// <summary>Monto gastado hasta la fecha dentro del periodo del presupuesto.</summary>
    public decimal Spent { get; set; }
    /// <summary>Periodicidad del presupuesto (ej. Mensual, Anual).</summary>
    public BudgetPeriod Period { get; set; }
    /// <summary>Nombre legible de la periodicidad.</summary>
    public string PeriodName => Period.ToString();
    /// <summary>Fecha de inicio del periodo del presupuesto.</summary>
    public DateTime StartDate { get; set; }
    /// <summary>Fecha de fin del periodo del presupuesto.</summary>
    public DateTime EndDate { get; set; }
    /// <summary>Indica si el presupuesto está activo.</summary>
    public bool IsActive { get; set; }
    /// <summary>Fecha de creación.</summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>Fecha de última actualización.</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>ID de la categoría a la que aplica el presupuesto.</summary>
    public Guid CategoryId { get; set; }
    /// <summary>Nombre de la categoría asociada.</summary>
    public string CategoryName { get; set; } = string.Empty;

    /// <summary>ID de la cuenta opcional a la que se limita el presupuesto.</summary>
    public Guid? AccountId { get; set; }
    /// <summary>Nombre de la cuenta asociada, si aplica.</summary>
    public string? AccountName { get; set; }

    // Propiedades calculadas para la UI
    /// <summary>Calcula el monto restante (Amount - Spent).</summary>
    public decimal Remaining => Amount - Spent;
    /// <summary>Calcula el porcentaje del presupuesto que se ha utilizado.</summary>
    public decimal PercentageUsed => Amount > 0 ? (Spent / Amount) * 100 : 0;
    /// <summary>Indica si se ha excedido el monto del presupuesto.</summary>
    public bool IsOverBudget => Spent > Amount;
}

/// <summary>
/// DTO utilizado para crear un nuevo presupuesto.
/// </summary>
public class CreateBudgetDto
{
    /// <summary>Nombre del nuevo presupuesto.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Descripción opcional.</summary>
    public string? Description { get; set; }
    /// <summary>Monto a asignar.</summary>
    public decimal Amount { get; set; }
    /// <summary>Periodicidad.</summary>
    public BudgetPeriod Period { get; set; }
    /// <summary>Fecha de inicio.</summary>
    public DateTime StartDate { get; set; }
    /// <summary>Fecha de fin.</summary>
    public DateTime EndDate { get; set; }
    /// <summary>ID de la categoría a presupuestar.</summary>
    public Guid CategoryId { get; set; }
    /// <summary>ID de la cuenta opcional a la que se aplica.</summary>
    public Guid? AccountId { get; set; }
}

/// <summary>
/// DTO utilizado para actualizar un presupuesto existente.
/// </summary>
public class UpdateBudgetDto
{
    /// <summary>Nombre del presupuesto.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Descripción opcional.</summary>
    public string? Description { get; set; }
    /// <summary>Monto del presupuesto.</summary>
    public decimal Amount { get; set; }
    /// <summary>Periodicidad.</summary>
    public BudgetPeriod Period { get; set; }
    /// <summary>Fecha de inicio.</summary>
    public DateTime StartDate { get; set; }
    /// <summary>Fecha de fin.</summary>
    public DateTime EndDate { get; set; }
    /// <summary>ID de la categoría asociada.</summary>
    public Guid CategoryId { get; set; }
    /// <summary>ID de la cuenta opcional asociada.</summary>
    public Guid? AccountId { get; set; }
    /// <summary>Estado de activación del presupuesto.</summary>
    public bool IsActive { get; set; }
}