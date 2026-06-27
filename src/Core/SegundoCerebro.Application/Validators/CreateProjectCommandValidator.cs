using FluentValidation;
using SegundoCerebro.Application.Features.Projects.Commands.CreateProject;

namespace SegundoCerebro.Application.Validators;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.ProjectDto.Name)
            .NotEmpty().WithMessage("El nombre del proyecto es requerido.")
            .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");
    }
}