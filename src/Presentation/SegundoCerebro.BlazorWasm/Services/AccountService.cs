using System.Net.Http.Json;
using SegundoCerebro.BlazorWasm.Models;

namespace SegundoCerebro.BlazorWasm.Services;

public class AccountService : ApiService<AccountDto, CreateAccountDto, UpdateAccountDto>, IAccountService
{
    public AccountService(HttpClient httpClient) : base(httpClient, "accounts")
    {
    }

    public async Task<decimal> GetTotalBalanceAsync()
    {
        var accounts = await GetActiveAccountsAsync();
        return accounts.Sum(a => a.Balance);
    }

    public async Task<IEnumerable<AccountDto>> GetActiveAccountsAsync()
    {
        var allAccounts = await GetAllAsync();
        return allAccounts.Where(a => a.IsActive);
    }
}