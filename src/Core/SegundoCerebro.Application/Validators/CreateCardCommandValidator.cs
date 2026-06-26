using FluentValidation;
using SegundoCerebro.Application.Features.Cards.Commands.CreateCard;

namespace SegundoCerebro.Application.Validators;

/// <summary>
/// Validador para el comando CreateCardCommand.
/// </summary>
public class CreateCardCommandValidator : AbstractValidator<CreateCardCommand>
{
    public CreateCardCommandValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("El ID de la cuenta es requerido.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre de la tarjeta es requerido.")
            .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

        RuleFor(x => x.StripePaymentMethodId)
            .NotEmpty().WithMessage("El ID del método de pago de Stripe es requerido.");
    }
}