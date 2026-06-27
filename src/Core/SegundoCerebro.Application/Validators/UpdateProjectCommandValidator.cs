using FluentValidation;
using SegundoCerebro.Application.Features.Projects.Commands.UpdateProject;

namespace SegundoCerebro.Application.Validators;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.ProjectDto.Name)
            .NotEmpty().WithMessage("El nombre del proyecto es requerido.")
            .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");
    }
}