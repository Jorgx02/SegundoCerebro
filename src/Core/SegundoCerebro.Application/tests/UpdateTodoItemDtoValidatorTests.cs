using FluentValidation.TestHelper;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Validators;
using SegundoCerebro.Domain.Enums;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Validators;

public class UpdateTodoItemDtoValidatorTests
{
    private readonly UpdateTodoItemDtoValidator _validator;

    public UpdateTodoItemDtoValidatorTests()
    {
        _validator = new UpdateTodoItemDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var model = new UpdateTodoItemDto { Title = string.Empty };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(t => t.Title);
    }

    [Fact]
    public void Should_Have_Error_When_Status_Is_Invalid()
    {
        var model = new UpdateTodoItemDto { Status = (TodoItemStatus)999 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(t => t.Status);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new UpdateTodoItemDto { Title = "Tarea act", Status = TodoItemStatus.NextAction };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}