using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Accounts.Queries.GetAllAccounts;

public record GetAllAccountsQuery : IRequest<IEnumerable<AccountDto>>;