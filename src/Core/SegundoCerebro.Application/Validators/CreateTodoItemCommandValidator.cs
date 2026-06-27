using FluentValidation;
using SegundoCerebro.Application.Features.TodoItems.Commands.CreateTodoItem;

namespace SegundoCerebro.Application.Validators;

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator()
    {
        RuleFor(x => x.TodoItemDto.Title)
            .NotEmpty().WithMessage("El título de la tarea es requerido.")
            .MaximumLength(200).WithMessage("El título no puede exceder los 200 caracteres.");
    }
}