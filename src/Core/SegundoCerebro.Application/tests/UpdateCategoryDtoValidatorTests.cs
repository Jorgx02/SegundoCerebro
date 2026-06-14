using FluentValidation.TestHelper;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Validators;
using SegundoCerebro.Domain.Enums;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Validators;

public class UpdateCategoryDtoValidatorTests
{
    private readonly UpdateCategoryDtoValidator _validator;

    public UpdateCategoryDtoValidatorTests()
    {
        _validator = new UpdateCategoryDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var model = new UpdateCategoryDto { Name = string.Empty };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Color_Is_Not_Hexadecimal()
    {
        var model = new UpdateCategoryDto { Color = "invalid-color" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(c => c.Color);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new UpdateCategoryDto
        {
            Name = "Transporte Actualizado",
            Color = "#0000FF",
            Icon = "bus-icon",
            Type = (CategoryType)1
        };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}