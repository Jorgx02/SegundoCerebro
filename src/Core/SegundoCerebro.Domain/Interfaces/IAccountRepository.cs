using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Domain.Interfaces;

/// <summary>
/// Define el contrato para el repositorio de Cuentas (Account), extendiendo el repositorio genérico.
/// Proporciona métodos de consulta específicos para la entidad Account.
/// </summary>
public interface IAccountRepository : IRepository<Account>
{
    /// <summary>
    /// Obtiene todas las cuentas activas.
    /// </summary>
    /// <returns>Una colección de cuentas activas.</returns>
    Task<IEnumerable<Account>> GetActiveAccountsAsync();

    /// <summary>
    /// Obtiene una cuenta por su número.
    /// </summary>
    /// <param name="accountNumber">El número de la cuenta.</param>
    /// <returns>La cuenta encontrada o null si no se encuentra.</returns>
    Task<Account?> GetByAccountNumberAsync(string accountNumber);

    /// <summary>
    /// Obtiene el saldo total de todas las cuentas.
    /// </summary>
    /// <returns>El saldo total de las cuentas.</returns>
    Task<decimal> GetTotalBalanceAsync();

    /// <summary>
    /// Obtiene todas las cuentas de un tipo específico.
    /// </summary>
    /// <param name="type">El tipo de cuenta.</param>
    /// <returns>Una colección de cuentas del tipo especificado.</returns>
    Task<IEnumerable<Account>> GetAccountsByTypeAsync(Domain.Enums.AccountType type);
}