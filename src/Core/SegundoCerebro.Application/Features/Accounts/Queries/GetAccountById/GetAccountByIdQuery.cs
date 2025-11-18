using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Accounts.Queries.GetAccountById;

public record GetAccountByIdQuery(Guid Id) : IRequest<AccountDto?>;