using FluentValidation;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Validators;

/// <summary>
/// Validador para el DTO de actualización de una tarea (`UpdateTodoItemDto`).
/// Define las reglas de negocio que debe cumplir una tarea para ser actualizada.
/// </summary>
public class UpdateTodoItemDtoValidator : AbstractValidator<UpdateTodoItemDto>
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UpdateTodoItemDtoValidator"/>.
    /// </summary>
    public UpdateTodoItemDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título es obligatorio")
            .MaximumLength(200).WithMessage("El título no puede exceder 200 caracteres");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("La descripción no puede exceder 1000 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Estado de tarea inválido");
    }
}