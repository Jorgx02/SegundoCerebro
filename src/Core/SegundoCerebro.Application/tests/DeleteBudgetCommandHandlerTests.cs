using FluentAssertions;
using Moq;
using SegundoCerebro.Application.Features.Budgets.Commands.DeleteBudget;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Budgets.Commands;

public class DeleteBudgetCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteBudgetCommandHandler _handler;

    public DeleteBudgetCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var budgetRepoMock = new Mock<IBudgetRepository>();
        _unitOfWorkMock.Setup(u => u.Budgets).Returns(budgetRepoMock.Object);

        _handler = new DeleteBudgetCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingBudget_ShouldSoftDeleteAndReturnTrue()
    {
        // Arrange
        var budgetId = Guid.NewGuid();
        var command = new DeleteBudgetCommand(budgetId);
        var existingBudget = new Budget { Id = budgetId, IsActive = true };

        _unitOfWorkMock.Setup(u => u.Budgets.GetByIdAsync(budgetId)).ReturnsAsync(existingBudget);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        existingBudget.IsActive.Should().BeFalse(); // Verificamos que hace el borrado lógico
        existingBudget.UpdatedAt.Should().NotBeNull();

        _unitOfWorkMock.Verify(u => u.Budgets.UpdateAsync(existingBudget), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingBudget_ShouldReturnFalse()
    {
        // Arrange
        var budgetId = Guid.NewGuid();
        var command = new DeleteBudgetCommand(budgetId);
        _unitOfWorkMock.Setup(u => u.Budgets.GetByIdAsync(budgetId)).ReturnsAsync((Budget?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        _unitOfWorkMock.Verify(u => u.Budgets.UpdateAsync(It.IsAny<Budget>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
    }
}