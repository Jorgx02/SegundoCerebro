using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Accounts.Queries.GetAccountById;

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, AccountDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAccountByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AccountDto?> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await _unitOfWork.Accounts.GetByIdAsync(request.Id);
        return account == null ? null : _mapper.Map<AccountDto>(account);
    }
}