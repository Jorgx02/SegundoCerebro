using FluentValidation.TestHelper;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Validators;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Validators;

public class UpdateAccountDtoValidatorTests
{
    private readonly UpdateAccountDtoValidator _validator;

    public UpdateAccountDtoValidatorTests()
    {
        _validator = new UpdateAccountDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var model = new UpdateAccountDto { Name = string.Empty };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(account => account.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Balance_Is_Negative()
    {
        var model = new UpdateAccountDto { Balance = -100m };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(account => account.Balance);
    }

    [Fact]
    public void Should_Have_Error_When_Currency_Is_Not_3_Characters()
    {
        var model = new UpdateAccountDto { Currency = "EURO" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(account => account.Currency);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new UpdateAccountDto { Name = "Cuenta Principal", Currency = "EUR", Balance = 1500m };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(account => account.Name);
        result.ShouldNotHaveValidationErrorFor(account => account.Currency);
        result.ShouldNotHaveValidationErrorFor(account => account.Balance);
    }
}