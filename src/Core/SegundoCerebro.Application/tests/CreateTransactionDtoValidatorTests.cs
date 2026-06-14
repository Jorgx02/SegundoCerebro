using FluentValidation.TestHelper;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Validators;
using SegundoCerebro.Domain.Enums;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Validators;

public class CreateTransactionDtoValidatorTests
{
    private readonly CreateTransactionDtoValidator _validator;

    public CreateTransactionDtoValidatorTests()
    {
        _validator = new CreateTransactionDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        var model = new CreateTransactionDto { Description = string.Empty };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(t => t.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Amount_Is_Zero_Or_Less()
    {
        var model = new CreateTransactionDto { Amount = 0 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(t => t.Amount);
    }

    [Fact]
    public void Should_Have_Error_When_Date_Is_Future()
    {
        var model = new CreateTransactionDto { Date = DateTime.Now.AddDays(2) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(t => t.Date);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new CreateTransactionDto
        {
            Description = "Supermercado",
            Amount = 50m,
            Type = TransactionType.Expense,
            Date = DateTime.Now,
            AccountId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid()
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}