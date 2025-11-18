using FluentValidation;
using SegundoCerebro.BlazorWasm.Models;

namespace SegundoCerebro.BlazorWasm.Validators;

public class CreateAccountDtoValidator : AbstractValidator<CreateAccountDto>
{
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
    }
}

public class UpdateAccountDtoValidator : AbstractValidator<UpdateAccountDto>
{
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
    }
}