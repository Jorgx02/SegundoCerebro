using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Transactions.Queries.GetAllTransactions;

public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, IEnumerable<TransactionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllTransactionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TransactionDto>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _unitOfWork.Transactions.GetAllAsync();
        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }
}