using FluentValidation;
using SegundoCerebro.Application.Features.TodoItems.Commands.UpdateTodoItem;

namespace SegundoCerebro.Application.Validators;

public class UpdateTodoItemCommandValidator : AbstractValidator<UpdateTodoItemCommand>
{
    public UpdateTodoItemCommandValidator()
    {
        RuleFor(x => x.TodoItemDto.Title)
            .NotEmpty().WithMessage("El título de la tarea es requerido.")
            .MaximumLength(200).WithMessage("El título no puede exceder los 200 caracteres.");
    }
}
