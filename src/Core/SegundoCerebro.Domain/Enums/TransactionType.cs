namespace SegundoCerebro.Domain.Enums;

/// <summary>
/// Define el tipo de movimiento de dinero que representa una transacción.
/// </summary>
public enum TransactionType
{
    /// <summary>Representa una entrada de dinero en una cuenta.</summary>
    Income = 1,

    /// <summary>Representa una salida de dinero de una cuenta.</summary>
    Expense = 2,

    /// <summary>Representa un movimiento de dinero entre dos cuentas del mismo usuario.</summary>
    Transfer = 3
}