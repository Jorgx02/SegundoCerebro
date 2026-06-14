using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Budgets.Commands.UpdateBudget;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Budgets.Commands;

public class UpdateBudgetCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateBudgetCommandHandler _handler;

    public UpdateBudgetCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var budgetRepoMock = new Mock<IBudgetRepository>();
        _unitOfWorkMock.Setup(u => u.Budgets).Returns(budgetRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new UpdateBudgetCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingBudget_ShouldUpdateAndReturnDto()
    {
        // Arrange
        var budgetId = Guid.NewGuid();
        var updateDto = new UpdateBudgetDto { Name = "Presupuesto Modificado", Amount = 600m };
        var command = new UpdateBudgetCommand(budgetId, updateDto);

        var existingBudget = new Budget { Id = budgetId, Name = "Presupuesto Viejo", Amount = 400m };
        var expectedDto = new BudgetDto { Id = budgetId, Name = "Presupuesto Modificado", Amount = 600m };

        _unitOfWorkMock.Setup(u => u.Budgets.GetByIdAsync(budgetId)).ReturnsAsync(existingBudget);

        // Configurar AutoMapper para que copie las propiedades al objeto existente
        _mapperMock.Setup(m => m.Map(updateDto, existingBudget)).Callback<UpdateBudgetDto, Budget>((src, dest) =>
        {
            dest.Name = src.Name;
            dest.Amount = src.Amount;
        });

        _mapperMock.Setup(m => m.Map<BudgetDto>(existingBudget)).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        result.Name.Should().Be("Presupuesto Modificado");
        result.Amount.Should().Be(600m);

        existingBudget.Name.Should().Be("Presupuesto Modificado");
        existingBudget.UpdatedAt.Should().NotBeNull();

        _unitOfWorkMock.Verify(u => u.Budgets.UpdateAsync(existingBudget), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingBudget_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var budgetId = Guid.NewGuid();
        var command = new UpdateBudgetCommand(budgetId, new UpdateBudgetDto());
        _unitOfWorkMock.Setup(u => u.Budgets.GetByIdAsync(budgetId)).ReturnsAsync((Budget?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        _unitOfWorkMock.Verify(u => u.Budgets.UpdateAsync(It.IsAny<Budget>()), Times.Never);
    }
}