using FluentValidation.TestHelper;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Validators;
using SegundoCerebro.Domain.Enums;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Validators;

public class CreateCategoryDtoValidatorTests
{
    private readonly CreateCategoryDtoValidator _validator;

    public CreateCategoryDtoValidatorTests()
    {
        _validator = new CreateCategoryDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var model = new CreateCategoryDto { Name = string.Empty };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Color_Is_Not_Hexadecimal()
    {
        // El validador exige formato #RRGGBB
        var model = new CreateCategoryDto { Color = "Rojo" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(c => c.Color);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new CreateCategoryDto
        {
            Name = "Transporte",
            Color = "#FF0000",
            Icon = "car-icon",
            Type = (CategoryType)1
        };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}