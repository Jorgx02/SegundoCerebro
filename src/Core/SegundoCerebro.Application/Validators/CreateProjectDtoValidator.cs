using FluentValidation;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Validators;

/// <summary>
/// Validador para el DTO de creación de un proyecto (`CreateProjectDto`).
/// Define las reglas de negocio que debe cumplir un proyecto para ser creado.
/// </summary>
public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CreateProjectDtoValidator"/>.
    /// </summary>
    public CreateProjectDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del proyecto es obligatorio")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("La fecha de fin debe ser posterior a la fecha de inicio")
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue);
    }
}