using FluentValidation;
using SegundoCerebro.BlazorWasm.Models.Commands;

namespace SegundoCerebro.BlazorWasm.Validators;

public class CreateTodoItemDtoValidator : AbstractValidator<CreateTodoItemDto>
{
    public CreateTodoItemDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título de la tarea es requerido.")
            .MaximumLength(200).WithMessage("El título no puede exceder los 200 caracteres.");
    }
}