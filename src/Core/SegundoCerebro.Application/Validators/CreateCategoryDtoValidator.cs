using FluentValidation;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Validators;

/// <summary>
/// Validador para el DTO de creación de una categoría (`CreateCategoryDto`).
/// Define las reglas de negocio que debe cumplir una categoría para ser creada.
/// </summary>
public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CreateCategoryDtoValidator"/>.
    /// </summary>
    public CreateCategoryDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre de la categoría es obligatorio")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Tipo de categoría inválido");

        RuleFor(x => x.Color)
            .NotEmpty().WithMessage("El color es obligatorio")
            .Matches(@"^#[0-9A-Fa-f]{6}$").WithMessage("El color debe estar en formato hexadecimal (#RRGGBB)");

        RuleFor(x => x.Icon)
            .NotEmpty().WithMessage("El icono es obligatorio")
            .MaximumLength(50).WithMessage("El icono no puede exceder 50 caracteres");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}