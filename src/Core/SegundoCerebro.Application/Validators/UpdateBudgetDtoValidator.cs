using FluentValidation;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Validators;

public class UpdateBudgetDtoValidator : AbstractValidator<UpdateBudgetDto>
{
    public UpdateBudgetDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del presupuesto es obligatorio")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("El monto del presupuesto debe ser mayor a cero");

        RuleFor(x => x.Period)
            .IsInEnum().WithMessage("Período de presupuesto inválido");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("La fecha de inicio es obligatoria");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("La fecha de fin es obligatoria")
            .GreaterThan(x => x.StartDate).WithMessage("La fecha de fin debe ser posterior a la fecha de inicio");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("La categoría es obligatoria");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}