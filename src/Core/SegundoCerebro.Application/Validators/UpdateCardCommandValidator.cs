using FluentValidation;
using SegundoCerebro.Application.Features.Cards.Commands.UpdateCard;

namespace SegundoCerebro.Application.Validators;

/// <summary>
/// Validador para el comando UpdateCardCommand.
/// </summary>
public class UpdateCardCommandValidator : AbstractValidator<UpdateCardCommand>
{
    public UpdateCardCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("El ID de la tarjeta es requerido.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre de la tarjeta es requerido.")
            .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

        RuleFor(x => x.ExpirationMonth)
            .InclusiveBetween(1, 12).WithMessage("El mes de expiración debe estar entre 1 y 12.");

        RuleFor(x => x.ExpirationYear)
            .GreaterThanOrEqualTo(DateTime.Now.Year).WithMessage("El año de expiración no puede ser en el pasado.");
    }
}