// filepath: src/Core/SegundoCerebro.Application/DTOs/FinancialSummaryDto.cs
namespace SegundoCerebro.Application.DTOs;

/// <summary>
/// DTO que encapsula un resumen financiero completo para un período de tiempo.
/// Utilizado principalmente en el Dashboard.
/// </summary>
public class FinancialSummaryDto
{
    /// <summary>Saldo total combinado de todas las cuentas activas.</summary>
    public decimal TotalBalance { get; set; }
    /// <summary>Suma total de todos los ingresos en el período.</summary>
    public decimal TotalIncome { get; set; }
    /// <summary>Suma total de todos los gastos en el período.</summary>
    public decimal TotalExpenses { get; set; }
    /// <summary>Resultado neto (Ingresos - Gastos).</summary>
    public decimal NetIncome { get; set; }
    /// <summary>Número de cuentas activas.</summary>
    public int ActiveAccounts { get; set; }
    /// <summary>Número total de transacciones en el período.</summary>
    public int TotalTransactions { get; set; }
    /// <summary>Desglose de gastos por categoría.</summary>
    public List<CategoryBreakdownDto> CategoryBreakdown { get; set; } = new(); // CAMBIADO
    /// <summary>Tendencia de ingresos y gastos a lo largo de los meses.</summary>
    public List<MonthlyTrendDto> MonthlyTrends { get; set; } = new();
}

/// <summary>
/// DTO que representa el desglose de gastos para una categoría específica.
/// </summary>
public class CategoryBreakdownDto
{
    /// <summary>ID de la categoría.</summary>
    public Guid CategoryId { get; set; }
    /// <summary>Nombre de la categoría.</summary>
    public string CategoryName { get; set; } = string.Empty;
    /// <summary>Monto total gastado en esta categoría.</summary>
    public decimal Amount { get; set; }
    /// <summary>Número de transacciones en esta categoría.</summary>
    public int TransactionCount { get; set; }
    /// <summary>Porcentaje que representa este gasto sobre el total de gastos.</summary>
    public decimal Percentage { get; set; }
}

/// <summary>
/// DTO que representa la tendencia financiera para un mes específico.
/// </summary>
public class MonthlyTrendDto
{
    /// <summary>Año del registro.</summary>
    public int Year { get; set; }
    /// <summary>Número del mes (1-12).</summary>
    public int Month { get; set; }
    /// <summary>Nombre del mes.</summary>
    public string MonthName { get; set; } = string.Empty;
    /// <summary>Total de ingresos en el mes.</summary>
    public decimal Income { get; set; }
    /// <summary>Total de gastos en el mes.</summary>
    public decimal Expenses { get; set; }
    /// <summary>Resultado neto del mes.</summary>
    public decimal Net { get; set; }
}