using FluentValidation.TestHelper;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Validators;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Validators;

public class CreateTodoItemDtoValidatorTests
{
    private readonly CreateTodoItemDtoValidator _validator;

    public CreateTodoItemDtoValidatorTests()
    {
        _validator = new CreateTodoItemDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var model = new CreateTodoItemDto { Title = string.Empty };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(t => t.Title);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new CreateTodoItemDto { Title = "Nueva tarea" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}