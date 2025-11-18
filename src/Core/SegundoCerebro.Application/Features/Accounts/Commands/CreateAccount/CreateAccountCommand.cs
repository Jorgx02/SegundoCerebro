using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Accounts.Commands.CreateAccount;

public record CreateAccountCommand(CreateAccountDto Account) : IRequest<AccountDto>;