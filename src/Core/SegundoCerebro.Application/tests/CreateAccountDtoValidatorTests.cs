using FluentValidation.TestHelper;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Validators;
using SegundoCerebro.Domain.Enums;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Validators;

public class CreateAccountDtoValidatorTests
{
    private readonly CreateAccountDtoValidator _validator;

    public CreateAccountDtoValidatorTests()
    {
        _validator = new CreateAccountDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var model = new CreateAccountDto { Name = string.Empty };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(account => account.Name);
    }

    [Fact]
    public void Should_Have_Error_When_InitialBalance_Is_Negative()
    {
        var model = new CreateAccountDto { InitialBalance = -100m };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(account => account.InitialBalance);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new CreateAccountDto
        {
            Name = "Cuenta Nómina",
            Currency = "EUR",
            InitialBalance = 1500m,
            Type = AccountType.Checking
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(account => account.Name);
        result.ShouldNotHaveValidationErrorFor(account => account.InitialBalance);
        result.ShouldNotHaveValidationErrorFor(account => account.Currency);
    }
}