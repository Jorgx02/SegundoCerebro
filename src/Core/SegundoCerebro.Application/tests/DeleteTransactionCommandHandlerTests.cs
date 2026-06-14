using FluentAssertions;
using Moq;
using SegundoCerebro.Application.Features.Transactions.Commands.DeleteTransaction;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Transactions.Commands;

public class DeleteTransactionCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteTransactionCommandHandler _handler;

    public DeleteTransactionCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var accountRepoMock = new Mock<IAccountRepository>();
        var transactionRepoMock = new Mock<ITransactionRepository>();
        var budgetRepoMock = new Mock<IBudgetRepository>();

        _unitOfWorkMock.Setup(u => u.Accounts).Returns(accountRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Transactions).Returns(transactionRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Budgets).Returns(budgetRepoMock.Object);

        _handler = new DeleteTransactionCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingExpenseTransaction_ShouldDeleteAndRestoreAccountBalance()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var accountId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        // Gasto de 100 que vamos a eliminar. La cuenta ahora tiene 400.
        var transaction = new Transaction { Id = transactionId, AccountId = accountId, CategoryId = categoryId, Amount = 100m, Type = TransactionType.Expense };
        var account = new Account { Id = accountId, Balance = 400m };
        var budget = new Budget { Id = Guid.NewGuid(), Spent = 200m }; // El presupuesto tenía 200 gastados

        var command = new DeleteTransactionCommand(transactionId);

        _unitOfWorkMock.Setup(u => u.Transactions.GetByIdAsync(transactionId)).ReturnsAsync(transaction);
        _unitOfWorkMock.Setup(u => u.Accounts.GetByIdAsync(accountId)).ReturnsAsync(account);
        _unitOfWorkMock.Setup(u => u.Budgets.GetBudgetByCategoryAndPeriodAsync(categoryId, transaction.Date)).ReturnsAsync(budget);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        // Como eliminamos un gasto de 100, la cuenta recupera ese dinero (400 + 100 = 500)
        account.Balance.Should().Be(500m);
        account.UpdatedAt.Should().NotBeNull();

        // Verificamos que se llamó a actualizar la cuenta y al presupuesto (pasando el importe en negativo)
        _unitOfWorkMock.Verify(u => u.Accounts.UpdateAsync(account), Times.Once);
        _unitOfWorkMock.Verify(u => u.Budgets.UpdateBudgetSpentAsync(budget.Id, -100m), Times.Once);

        _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.Transactions.DeleteAsync(transaction), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingTransaction_ShouldReturnFalse()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var command = new DeleteTransactionCommand(transactionId);

        _unitOfWorkMock.Setup(u => u.Transactions.GetByIdAsync(transactionId)).ReturnsAsync((Transaction?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
        // Al devolver false directamente, no se llama a Delete ni a Commit.
        _unitOfWorkMock.Verify(u => u.Transactions.DeleteAsync(It.IsAny<Transaction>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(), Times.Never);
    }
}