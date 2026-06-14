using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Transactions.Commands.CreateTransaction;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Transactions.Commands;

public class CreateTransactionCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateTransactionCommandHandler _handler;

    public CreateTransactionCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        // Configurar los repositorios que se usan en el handler de transacciones
        var accountRepoMock = new Mock<IAccountRepository>();
        var budgetRepoMock = new Mock<IBudgetRepository>();
        var transactionRepoMock = new Mock<ITransactionRepository>();

        _unitOfWorkMock.Setup(u => u.Accounts).Returns(accountRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Budgets).Returns(budgetRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Transactions).Returns(transactionRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new CreateTransactionCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidIncomeTransaction_ShouldIncreaseAccountBalance()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var account = new Account { Id = accountId, Balance = 1000m };

        var createDto = new CreateTransactionDto
        {
            AccountId = accountId,
            Amount = 500m,
            Type = TransactionType.Income,
            Description = "Salario"
        };
        var command = new CreateTransactionCommand(createDto);
        var transactionEntity = new Transaction { AccountId = accountId, Amount = 500m, Type = TransactionType.Income };
        var expectedDto = new TransactionDto { Id = Guid.NewGuid(), Amount = 500m };

        // Configurar comportamiento simulado
        _unitOfWorkMock.Setup(u => u.Accounts.GetByIdAsync(accountId)).ReturnsAsync(account);
        _mapperMock.Setup(m => m.Map<Transaction>(createDto)).Returns(transactionEntity);
        _unitOfWorkMock.Setup(u => u.Transactions.AddAsync(It.IsAny<Transaction>())).ReturnsAsync(transactionEntity);
        _mapperMock.Setup(m => m.Map<TransactionDto>(It.IsAny<Transaction>())).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        account.Balance.Should().Be(1500m); // Verificamos que se sumó el dinero a la cuenta

        // Verificamos el flujo de la base de datos
        _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.Accounts.UpdateAsync(account), Times.Once);
        _unitOfWorkMock.Verify(u => u.Transactions.AddAsync(It.IsAny<Transaction>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_AccountNotFound_ShouldThrowExceptionAndRollback()
    {
        // Arrange
        var createDto = new CreateTransactionDto { AccountId = Guid.NewGuid() };
        var command = new CreateTransactionCommand(createDto);
        _mapperMock.Setup(m => m.Map<Transaction>(createDto)).Returns(new Transaction { AccountId = createDto.AccountId });
        _unitOfWorkMock.Setup(u => u.Accounts.GetByIdAsync(createDto.AccountId)).ReturnsAsync((Account?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        _unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(), Times.Never);
    }
}