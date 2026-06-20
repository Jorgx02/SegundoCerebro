using FluentValidation;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Validators;

/// <summary>
/// Validador para el DTO de creación de una tarea (`CreateTodoItemDto`).
/// Define las reglas de negocio que debe cumplir una tarea para ser creada.
/// </summary>
public class CreateTodoItemDtoValidator : AbstractValidator<CreateTodoItemDto>
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CreateTodoItemDtoValidator"/>.
    /// </summary>
    public CreateTodoItemDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título es obligatorio")
            .MaximumLength(200).WithMessage("El título no puede exceder 200 caracteres");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("La descripción no puede exceder 1000 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}