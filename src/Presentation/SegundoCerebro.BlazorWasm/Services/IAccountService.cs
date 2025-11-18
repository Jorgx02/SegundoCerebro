using SegundoCerebro.BlazorWasm.Models;

namespace SegundoCerebro.BlazorWasm.Services;

public interface IAccountService : IApiService<AccountDto, CreateAccountDto, UpdateAccountDto>
{
    Task<decimal> GetTotalBalanceAsync();
    Task<IEnumerable<AccountDto>> GetActiveAccountsAsync();
}