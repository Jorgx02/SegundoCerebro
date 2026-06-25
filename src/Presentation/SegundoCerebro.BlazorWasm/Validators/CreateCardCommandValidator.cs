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

        // En una app real, Brand y Last4Digits los proporcionaría Stripe.
        // Para este TFG, permitimos la entrada manual.
        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("La marca es requerida (ej. Visa, Mastercard).");

        RuleFor(x => x.Last4Digits)
            .NotEmpty().WithMessage("Los últimos 4 dígitos son requeridos.")
            .Length(4).WithMessage("Debe contener exactamente 4 dígitos.")
            .Matches("^[0-9]{4}$").WithMessage("Debe contener solo números.");

        RuleFor(x => x.ExpirationMonth)
            .InclusiveBetween(1, 12).WithMessage("El mes debe estar entre 1 y 12.");

        RuleFor(x => x.ExpirationYear)
            .GreaterThanOrEqualTo(DateTime.Now.Year).WithMessage("El año no puede ser en el pasado.")
            .LessThanOrEqualTo(DateTime.Now.Year + 20).WithMessage("El año de expiración parece demasiado lejano.");
    }
}