using Microsoft.EntityFrameworkCore;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Data;

namespace SegundoCerebro.Infrastructure.Repositories;

public class AccountRepository : Repository<Account>, IAccountRepository
{
    public AccountRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Account>> GetActiveAccountsAsync()
    {
        return await _dbSet.Where(a => a.IsActive).ToListAsync();
    }

    public async Task<Account?> GetByAccountNumberAsync(string accountNumber)
    {
        return await _dbSet.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
    }

    public async Task<decimal> GetTotalBalanceAsync()
    {
        return await _dbSet.Where(a => a.IsActive).SumAsync(a => a.Balance);
    }

    public async Task<IEnumerable<Account>> GetAccountsByTypeAsync(Domain.Enums.AccountType type)
    {
        return await _dbSet.Where(a => a.Type == type && a.IsActive).ToListAsync();
    }
}