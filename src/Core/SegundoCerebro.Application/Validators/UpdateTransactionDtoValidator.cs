using FluentValidation;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Validators;

/// <summary>
/// Validador para el DTO de actualización de una transacción (`UpdateTransactionDto`).
/// Define las reglas de negocio que debe cumplir una transacción para ser actualizada.
/// </summary>
public class UpdateTransactionDtoValidator : AbstractValidator<UpdateTransactionDto>
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UpdateTransactionDtoValidator"/>.
    /// </summary>
    public UpdateTransactionDtoValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción es obligatoria")
            .MaximumLength(200).WithMessage("La descripción no puede exceder 200 caracteres");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("El monto debe ser mayor a cero");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Tipo de transacción inválido");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("La fecha es obligatoria");

        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("La cuenta es obligatoria");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("La categoría es obligatoria");

        RuleFor(x => x.Reference)
            .MaximumLength(100).WithMessage("La referencia no puede exceder 100 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Reference));
    }
}