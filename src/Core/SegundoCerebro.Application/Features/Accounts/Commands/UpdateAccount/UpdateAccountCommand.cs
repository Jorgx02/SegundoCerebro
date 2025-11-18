using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Accounts.Commands.UpdateAccount;

public record UpdateAccountCommand(Guid Id, UpdateAccountDto Account) : IRequest<AccountDto>;