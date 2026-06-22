namespace SegundoCerebro.Domain.Enums;

/// <summary>
/// Define los diferentes tipos de cuentas financieras que un usuario puede gestionar.
/// </summary>
public enum AccountType
{
    /// <summary>Cuenta corriente para operaciones diarias.</summary>
    Checking = 1,

    /// <summary>Cuenta de ahorro, generalmente con menos operativa.</summary>
    Savings = 2,

    /// <summary>Representa el estado de una tarjeta de crédito.</summary>
    CreditCard = 3,

    /// <summary>Cuenta destinada a la gestión de inversiones.</summary>
    Investment = 4,

    /// <summary>Dinero físico que el usuario gestiona.</summary>
    Cash = 5
}