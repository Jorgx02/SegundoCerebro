using FluentValidation;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Validators;

/// <summary>
/// Validador para el DTO de actualización de un proyecto (`UpdateProjectDto`).
/// Define las reglas de negocio que debe cumplir un proyecto para ser actualizado.
/// </summary>
public class UpdateProjectDtoValidator : AbstractValidator<UpdateProjectDto>
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UpdateProjectDtoValidator"/>.
    /// </summary>
    public UpdateProjectDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del proyecto es obligatorio")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Estado de proyecto inválido");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate).WithMessage("La fecha de fin debe ser posterior a la fecha de inicio")
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue);
    }
}