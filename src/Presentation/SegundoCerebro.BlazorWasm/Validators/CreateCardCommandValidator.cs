using FluentValidation;
using SegundoCerebro.BlazorWasm.Models.Commands;

namespace SegundoCerebro.BlazorWasm.Validators;

public class CreateCardCommandValidator : AbstractValidator<CreateCardCommand>
{
    public CreateCardCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre de la tarjeta es requerido.")
            .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

        // El nombre del titular es requerido por Stripe para crear el PaymentMethod.
        RuleFor(x => x.CardholderName)
            .NotEmpty().WithMessage("El nombre del titular es requerido.")
            .MaximumLength(100).WithMessage("El nombre del titular no puede exceder los 100 caracteres.");
    }
}