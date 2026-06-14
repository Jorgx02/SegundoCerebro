using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Transactions.Commands.UpdateTransaction;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Transactions.Commands;

public class UpdateTransactionCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateTransactionCommandHandler _handler;

    public UpdateTransactionCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var accountRepoMock = new Mock<IAccountRepository>();
        var transactionRepoMock = new Mock<ITransactionRepository>();

        _unitOfWorkMock.Setup(u => u.Accounts).Returns(accountRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Transactions).Returns(transactionRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new UpdateTransactionCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingExpenseTransaction_ShouldUpdateAmountAndAdjustAccountBalance()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var accountId = Guid.NewGuid();

        // Supongamos que era un gasto de 50, y la cuenta quedó en 950.
        var existingTransaction = new Transaction { Id = transactionId, AccountId = accountId, Amount = 50m, Type = TransactionType.Expense };
        var account = new Account { Id = accountId, Balance = 950m };

        // Ahora lo actualizamos a un gasto de 70.
        var updateDto = new UpdateTransactionDto { AccountId = accountId, Amount = 70m, Type = TransactionType.Expense };
        var command = new UpdateTransactionCommand(transactionId, updateDto);
        var expectedDto = new TransactionDto { Id = transactionId, Amount = 70m };

        _unitOfWorkMock.Setup(u => u.Transactions.GetByIdAsync(transactionId)).ReturnsAsync(existingTransaction);
        _unitOfWorkMock.Setup(u => u.Accounts.GetByIdAsync(accountId)).ReturnsAsync(account);

        // Simulamos AutoMapper mapeando el nuevo valor a la transacción existente
        _mapperMock.Setup(m => m.Map(updateDto, existingTransaction)).Callback<UpdateTransactionDto, Transaction>((src, dest) =>
        {
            dest.Amount = src.Amount;
        });

        _mapperMock.Setup(m => m.Map<TransactionDto>(existingTransaction)).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        // El balance debería hacer: 950 + 50 (revertir) = 1000. Luego 1000 - 70 (aplicar nuevo) = 930m
        account.Balance.Should().Be(930m);

        _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.Accounts.UpdateAsync(account), Times.Once); // Se actualiza 1 vez al final del proceso
        _unitOfWorkMock.Verify(u => u.Transactions.UpdateAsync(existingTransaction), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_TransactionNotFound_ShouldThrowKeyNotFoundExceptionAndRollback()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var command = new UpdateTransactionCommand(transactionId, new UpdateTransactionDto());

        _unitOfWorkMock.Setup(u => u.Transactions.GetByIdAsync(transactionId)).ReturnsAsync((Transaction?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));

        _unitOfWorkMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(), Times.Never);
    }
}