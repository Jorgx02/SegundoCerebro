using AutoMapper;
using FluentValidation;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Exceptions;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Budgets.Commands.CreateBudget;

/// <summary>
/// Manejador para el comando de creación de un presupuesto.
/// </summary>
public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, BudgetDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBudgetCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Procesa la solicitud de creación de un presupuesto.
    /// </summary>
    /// <param name="request">El comando con los datos del presupuesto.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El DTO del presupuesto recién creado.</returns>
    public async Task<BudgetDto> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar que la categoría existe y es de tipo 'Expense'
        var category = await _unitOfWork.Categories.GetByIdAsync(request.Budget.CategoryId);
        if (category is null)
        {
            throw new NotFoundException(nameof(Category), request.Budget.CategoryId);
        }

        if (category.Type != CategoryType.Expense)
        {
            throw new ValidationException("Los presupuestos solo pueden crearse para categorías de gastos (Expense).");
        }

        // 2. Mapear y crear el presupuesto
        var budget = _mapper.Map<Budget>(request.Budget);
        budget.Id = Guid.NewGuid();
        budget.CreatedAt = DateTime.UtcNow;

        var createdBudget = await _unitOfWork.Budgets.AddAsync(budget);
        await _unitOfWork.SaveChangesAsync();

        // 3. Mapear y devolver el resultado
        return _mapper.Map<BudgetDto>(createdBudget);
    }
}