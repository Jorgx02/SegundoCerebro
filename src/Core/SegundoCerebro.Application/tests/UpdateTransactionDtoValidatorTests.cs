using FluentValidation.TestHelper;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Validators;
using SegundoCerebro.Domain.Enums;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Validators;

public class UpdateTransactionDtoValidatorTests
{
    private readonly UpdateTransactionDtoValidator _validator;

    public UpdateTransactionDtoValidatorTests()
    {
        _validator = new UpdateTransactionDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        var model = new UpdateTransactionDto { Description = string.Empty };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(t => t.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Amount_Is_Zero_Or_Less()
    {
        var model = new UpdateTransactionDto { Amount = 0 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(t => t.Amount);
    }

    [Fact]
    public void Should_Have_Error_When_AccountId_Is_Empty()
    {
        var model = new UpdateTransactionDto { AccountId = Guid.Empty };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(t => t.AccountId);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new UpdateTransactionDto
        {
            Description = "Compra actualizada",
            Amount = 75m,
            Type = TransactionType.Expense,
            Date = DateTime.Now,
            AccountId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid()
        };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}