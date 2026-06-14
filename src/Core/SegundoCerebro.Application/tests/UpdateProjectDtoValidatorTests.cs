using FluentValidation.TestHelper;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Validators;
using SegundoCerebro.Domain.Enums;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Validators;

public class UpdateProjectDtoValidatorTests
{
    private readonly UpdateProjectDtoValidator _validator;

    public UpdateProjectDtoValidatorTests()
    {
        _validator = new UpdateProjectDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var model = new UpdateProjectDto { Name = string.Empty };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(p => p.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Status_Is_Invalid()
    {
        var model = new UpdateProjectDto { Status = (ProjectStatus)999 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(p => p.Status);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new UpdateProjectDto { Name = "Proyecto TFG Activo", Status = ProjectStatus.Active };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}