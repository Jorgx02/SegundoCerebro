namespace SegundoCerebro.Domain.Enums;

/// <summary>
/// Enumeración que representa los diferentes tipos de cuentas financieras.
/// </summary>
public enum AccountType
{
    Checking = 1,    // Cuenta corriente
    Savings = 2,     // Cuenta de ahorro
    CreditCard = 3,  // Tarjeta de crédito
    Investment = 4,  // Cuenta de inversión
    Cash = 5         // Efectivo
}