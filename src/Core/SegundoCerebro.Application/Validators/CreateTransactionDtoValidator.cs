using FluentValidation;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Validators;

/// <summary>
/// Validador para el DTO de creación de una transacción (`CreateTransactionDto`).
/// Define las reglas de negocio que debe cumplir una transacción para ser creada.
/// </summary>
public class CreateTransactionDtoValidator : AbstractValidator<CreateTransactionDto>
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CreateTransactionDtoValidator"/>.
    /// </summary>
    public CreateTransactionDtoValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción es obligatoria")
            .MaximumLength(200).WithMessage("La descripción no puede exceder 200 caracteres");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("El monto debe ser mayor a cero");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Tipo de transacción inválido");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("La fecha es obligatoria")
            .LessThanOrEqualTo(DateTime.Now.Date.AddDays(1))
            .WithMessage("La fecha no puede ser futura");

        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("La cuenta es obligatoria");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("La categoría es obligatoria");

        RuleFor(x => x.Reference)
            .MaximumLength(100).WithMessage("La referencia no puede exceder 100 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Reference));
    }
}