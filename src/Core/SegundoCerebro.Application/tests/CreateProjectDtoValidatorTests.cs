using FluentValidation.TestHelper;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Validators;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Validators;

public class CreateProjectDtoValidatorTests
{
    private readonly CreateProjectDtoValidator _validator;

    public CreateProjectDtoValidatorTests()
    {
        _validator = new CreateProjectDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var model = new CreateProjectDto { Name = string.Empty };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(p => p.Name);
    }

    [Fact]
    public void Should_Have_Error_When_EndDate_Is_Before_StartDate()
    {
        var model = new CreateProjectDto { StartDate = new DateTime(2026, 2, 1), EndDate = new DateTime(2026, 1, 1) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(p => p.EndDate);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new CreateProjectDto { Name = "Proyecto TFG", Description = "App en Blazor", StartDate = new DateTime(2026, 1, 1), EndDate = new DateTime(2026, 6, 1) };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}