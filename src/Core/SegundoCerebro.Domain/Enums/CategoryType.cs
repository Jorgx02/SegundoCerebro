namespace SegundoCerebro.Domain.Enums;

/// <summary>
/// Define la naturaleza fundamental de una categoría, determinando si agrupa ingresos o gastos.
/// </summary>
public enum CategoryType
{
    /// <summary>Categoría para clasificar entradas de dinero.</summary>
    Income = 1,

    /// <summary>Categoría para clasificar salidas de dinero.</summary>
    Expense = 2
}