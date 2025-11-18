using MediatR;

namespace SegundoCerebro.Application.Features.Accounts.Commands.DeleteAccount;

public record DeleteAccountCommand(Guid Id) : IRequest<bool>;