using FluentValidation;

namespace SegundoCerebro.Application.Features.Cards.Commands.CreateCard;

/// <summary>
/// Validador para el comando CreateCardCommand.
/// </summary>
public class CreateCardDtoValidator : AbstractValidator<CreateCardCommand>
{
    public CreateCardDtoValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("El ID de la cuenta es requerido.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre de la tarjeta es requerido.")
            .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("La marca de la tarjeta es requerida.")
            .MaximumLength(50).WithMessage("La marca no puede exceder los 50 caracteres.");

        RuleFor(x => x.Last4Digits)
            .NotEmpty().WithMessage("Los últimos 4 dígitos son requeridos.")
            .Length(4).WithMessage("Debe proporcionar exactamente 4 dígitos.")
            .Matches("^[0-9]{4}$").WithMessage("Debe contener solo números.");

        RuleFor(x => x.ExpirationMonth)
            .InclusiveBetween(1, 12).WithMessage("El mes de expiración debe estar entre 1 y 12.");

        RuleFor(x => x.ExpirationYear)
            .GreaterThanOrEqualTo(DateTime.Now.Year).WithMessage("El año de expiración no puede ser en el pasado.");

        RuleFor(x => x.StripePaymentMethodId)
            .NotEmpty().WithMessage("El ID del método de pago de Stripe es requerido.");
    }
}