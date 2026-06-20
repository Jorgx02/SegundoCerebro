using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Accounts.Commands.CreateAccount;

/// <summary>
/// Comando para crear una nueva cuenta financiera.
/// </summary>
/// <param name="Account">DTO con los datos de la cuenta a crear.</param>
public record CreateAccountCommand(CreateAccountDto Account) : IRequest<AccountDto>;