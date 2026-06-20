using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Accounts.Queries.GetAccountById;

/// <summary>
/// Consulta para obtener una cuenta financiera específica por su ID.
/// </summary>
/// <param name="Id">El identificador único de la cuenta.</param>
public record GetAccountByIdQuery(Guid Id) : IRequest<AccountDto?>;