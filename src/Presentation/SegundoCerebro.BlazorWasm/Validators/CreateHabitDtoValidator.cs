using FluentValidation;
using SegundoCerebro.BlazorWasm.Models;

namespace SegundoCerebro.BlazorWasm.Validators;

/// <summary>
/// Validador para el DTO de creación de hábitos en el cliente.
/// </summary>
public class CreateHabitDtoValidator : AbstractValidator<CreateHabitDto>
{
    public CreateHabitDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La descripción no puede tener más de 500 caracteres.");
    }
}