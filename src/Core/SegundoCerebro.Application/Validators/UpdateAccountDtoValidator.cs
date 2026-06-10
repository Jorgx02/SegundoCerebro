using FluentValidation;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Validators;

/// <summary>
/// Validador de FluentValidation para el DTO de actualización de cuentas.
/// Asegura que los datos de entrada cumplan con las reglas de negocio en el backend.
/// </summary>
public class UpdateAccountDtoValidator : AbstractValidator<UpdateAccountDto>
{
    /// <summary>
    /// Define las reglas y restricciones para actualizar una cuenta de forma segura.
    /// </summary>
    public UpdateAccountDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre de la cuenta es obligatorio")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Tipo de cuenta inválido");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("La moneda es obligatoria")
            .Length(3).WithMessage("La moneda debe tener exactamente 3 caracteres");

        RuleFor(x => x.AccountNumber)
            .MaximumLength(50).WithMessage("El número de cuenta no puede exceder 50 caracteres")
            .When(x => !string.IsNullOrEmpty(x.AccountNumber));

        RuleFor(x => x.BankName)
            .MaximumLength(100).WithMessage("El nombre del banco no puede exceder 100 caracteres")
            .When(x => !string.IsNullOrEmpty(x.BankName));

        RuleFor(x => x.Balance)
            .GreaterThanOrEqualTo(0).WithMessage("El saldo no puede ser negativo");
    }
}