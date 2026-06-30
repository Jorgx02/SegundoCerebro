using FluentValidation;
using SegundoCerebro.Application.Features.Habits.Commands.UpdateHabit;

namespace SegundoCerebro.Application.Validators;

/// <summary>
/// Validador para el comando de actualización de un hábito.
/// </summary>
public class UpdateHabitCommandValidator : AbstractValidator<UpdateHabitCommand>
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UpdateHabitCommandValidator"/>.
    /// </summary>
    public UpdateHabitCommandValidator()
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