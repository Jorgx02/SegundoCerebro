using FluentValidation;
using SegundoCerebro.Application.Features.Habits.Commands.CreateHabit;

namespace SegundoCerebro.Application.Validators;

/// <summary>
/// Validador para el comando de creación de un hábito.
/// </summary>
public class CreateHabitCommandValidator : AbstractValidator<CreateHabitCommand>
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CreateHabitCommandValidator"/>.
    /// </summary>
    public CreateHabitCommandValidator()
    {
        RuleFor(v => v.HabitDto.Name)
            .NotEmpty().WithMessage("El nombre del hábito es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

        RuleFor(v => v.HabitDto.Description)
            .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres.");

        RuleFor(v => v.HabitDto.Frequency)
            .IsInEnum().WithMessage("La frecuencia especificada no es válida.");
    }
}