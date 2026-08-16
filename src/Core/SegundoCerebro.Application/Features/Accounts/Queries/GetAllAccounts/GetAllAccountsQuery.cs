using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Accounts.Queries.GetAllAccounts;

/// <summary>
/// Consulta para obtener todas las cuentas del usuario.
/// </summary>
public record GetAllAccountsQuery : IRequest<IEnumerable<AccountDto>>;