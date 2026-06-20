using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Accounts.Queries.GetAccountById;

/// <summary>
/// Manejador para la consulta que obtiene una cuenta por su ID.
/// </summary>
public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, AccountDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAccountByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Procesa la solicitud para obtener una cuenta por su ID.
    /// </summary>
    /// <param name="request">La consulta con el ID de la cuenta.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El DTO de la cuenta si se encuentra; de lo contrario, null.</returns>
    public async Task<AccountDto?> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Accounts.GetByIdAsync(request.Id);
        return account == null ? null : _mapper.Map<AccountDto>(account);
    }
}