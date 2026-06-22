namespace SegundoCerebro.Domain.Enums;

/// <summary>
/// Define la recurrencia o periodicidad de un presupuesto.
/// </summary>
public enum BudgetPeriod
{
    /// <summary>El presupuesto se reinicia cada semana.</summary>
    Weekly = 1,

    /// <summary>El presupuesto se reinicia cada mes (el más común).</summary>
    Monthly = 2,

    /// <summary>El presupuesto se reinicia cada trimestre.</summary>
    Quarterly = 3,

    /// <summary>El presupuesto se reinicia cada año.</summary>
    Yearly = 4,

    /// <summary>El presupuesto tiene un rango de fechas personalizado y no se reinicia automáticamente.</summary>
    Custom = 99
}