using FluentValidation.TestHelper;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Validators;
using SegundoCerebro.Domain.Enums;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Validators;

public class CreateBudgetDtoValidatorTests
{
    private readonly CreateBudgetDtoValidator _validator;

    public CreateBudgetDtoValidatorTests()
    {
        _validator = new CreateBudgetDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var model = new CreateBudgetDto { Name = string.Empty };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(b => b.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Amount_Is_Zero_Or_Less()
    {
        var model = new CreateBudgetDto { Amount = 0 };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(b => b.Amount);
    }

    [Fact]
    public void Should_Have_Error_When_EndDate_Is_Before_StartDate()
    {
        var model = new CreateBudgetDto
        {
            StartDate = new DateTime(2026, 1, 15),
            EndDate = new DateTime(2026, 1, 10) // Fecha de fin anterior a la de inicio
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(b => b.EndDate);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new CreateBudgetDto
        {
            Name = "Presupuesto Alimentación",
            Amount = 300m,
            Period = (BudgetPeriod)1, // Asumiendo que 1 es un valor válido (ej. Monthly)
            StartDate = new DateTime(2026, 1, 1),
            EndDate = new DateTime(2026, 1, 31),
            CategoryId = Guid.NewGuid()
        };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}