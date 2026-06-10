using FluentValidation;
using SegundoCerebro.BlazorWasm.Models;

namespace SegundoCerebro.BlazorWasm.Validators;

/// <summary>
/// Validador de FluentValidation para el DTO de creación de cuentas en el Frontend (Blazor).
/// Previene llamadas innecesarias a la API si los datos no son válidos.
/// </summary>
public class CreateAccountDtoValidator : AbstractValidator<CreateAccountDto>
{
    /// <summary>
    /// Define las reglas de validación en tiempo real para el formulario de creación.
    /// </summary>
    public CreateAccountDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre de la cuenta es obligatorio")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Tipo de cuenta inválido");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("La moneda es obligatoria")
            .Length(3).WithMessage("La moneda debe tener exactamente 3 caracteres");

        RuleFor(x => x.InitialBalance)
            .GreaterThanOrEqualTo(0).WithMessage("El saldo inicial no puede ser negativo");

        RuleFor(x => x.AccountNumber)
            .MaximumLength(50).WithMessage("El número de cuenta no puede exceder 50 caracteres")
            .When(x => !string.IsNullOrEmpty(x.AccountNumber));

        RuleFor(x => x.BankName)
            .MaximumLength(100).WithMessage("El nombre del banco no puede exceder 100 caracteres")
            .When(x => !string.IsNullOrEmpty(x.BankName));
    }
}

/// <summary>
/// Validador de FluentValidation para el DTO de actualización de cuentas en el Frontend.
/// </summary>
public class UpdateAccountDtoValidator : AbstractValidator<UpdateAccountDto>
{
    /// <summary>
    /// Define las reglas de validación en tiempo real para el formulario de edición.
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