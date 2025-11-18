using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Budgets.Queries.GetAllBudgets;

public class GetAllBudgetsQueryHandler : IRequestHandler<GetAllBudgetsQuery, IEnumerable<BudgetDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllBudgetsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BudgetDto>> Handle(GetAllBudgetsQuery request, CancellationToken cancellationToken)
    {
        var budgets = await _unitOfWork.Budgets.GetActiveBudgetsAsync();
        return _mapper.Map<IEnumerable<BudgetDto>>(budgets);
    }
}