using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Domain.Interfaces;

public interface IAccountRepository : IRepository<Account>
{
    Task<IEnumerable<Account>> GetActiveAccountsAsync();
    Task<Account?> GetByAccountNumberAsync(string accountNumber);
    Task<decimal> GetTotalBalanceAsync();
    Task<IEnumerable<Account>> GetAccountsByTypeAsync(Domain.Enums.AccountType type);
}