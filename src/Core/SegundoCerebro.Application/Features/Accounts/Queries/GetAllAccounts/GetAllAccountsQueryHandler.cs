using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Accounts.Queries.GetAllAccounts;

public class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccountsQuery, IEnumerable<AccountDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllAccountsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AccountDto>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _unitOfWork.Accounts.GetActiveAccountsAsync();
        return _mapper.Map<IEnumerable<AccountDto>>(accounts);
    }
}