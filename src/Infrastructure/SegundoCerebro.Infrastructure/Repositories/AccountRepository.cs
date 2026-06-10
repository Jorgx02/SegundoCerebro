using Microsoft.EntityFrameworkCore;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Data;

namespace SegundoCerebro.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio para la gestión de las cuentas bancarias/financieras.
/// </summary>
public class AccountRepository : Repository<Account>, IAccountRepository
{
    public AccountRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtiene todas las cuentas de la base de datos (tanto activas como inactivas),
    /// para permitir al frontend realizar los filtros correspondientes.
    /// </summary>
    public async Task<IEnumerable<Account>> GetActiveAccountsAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Busca una cuenta utilizando exclusivamente su número de IBAN.
    /// </summary>
    /// <param name="accountNumber">El número de cuenta (IBAN).</param>
    public async Task<Account?> GetByAccountNumberAsync(string accountNumber)
    {
        return await _dbSet.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
    }

    /// <summary>
    /// Calcula el patrimonio total sumando el balance de todas las cuentas que están Activas.
    /// </summary>
    /// <returns>La suma total en la divisa base (EUR).</returns>
    public async Task<decimal> GetTotalBalanceAsync()
    {
        return await _dbSet.Where(a => a.IsActive).SumAsync(a => a.Balance);
    }

    /// <summary>
    /// Obtiene todas las cuentas activas que pertenecen a un tipo específico.
    /// </summary>
    /// <param name="type">Tipo de cuenta (Checking, Savings, CreditCard, Investment, Cash).</param>
    public async Task<IEnumerable<Account>> GetAccountsByTypeAsync(Domain.Enums.AccountType type)
    {
        return await _dbSet.Where(a => a.Type == type && a.IsActive).ToListAsync();
    }
}