using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Budgets.Commands.CreateBudget;

public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, BudgetDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBudgetCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BudgetDto> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
    {
        var budget = _mapper.Map<Budget>(request.Budget);
        budget.Id = Guid.NewGuid();
        budget.CreatedAt = DateTime.UtcNow;

        var createdBudget = await _unitOfWork.Budgets.AddAsync(budget);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<BudgetDto>(createdBudget);
    }
}