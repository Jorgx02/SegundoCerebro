using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Budgets.Queries.GetBudgetById;

public class GetBudgetByIdQueryHandler : IRequestHandler<GetBudgetByIdQuery, BudgetDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBudgetByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BudgetDto?> Handle(GetBudgetByIdQuery request, CancellationToken cancellationToken)
    {
        var budget = await _unitOfWork.Budgets.GetByIdAsync(request.Id);
        return budget == null ? null : _mapper.Map<BudgetDto>(budget);
    }
}