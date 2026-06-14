using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Transactions.Queries.GetTransactionById;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Transactions.Queries;

public class GetTransactionByIdQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetTransactionByIdQueryHandler _handler;

    public GetTransactionByIdQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var transactionRepoMock = new Mock<ITransactionRepository>();
        _unitOfWorkMock.Setup(u => u.Transactions).Returns(transactionRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new GetTransactionByIdQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingTransaction_ShouldReturnDto()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var transaction = new Transaction { Id = transactionId, Amount = 150m };
        var expectedDto = new TransactionDto { Id = transactionId, Amount = 150m };

        _unitOfWorkMock.Setup(u => u.Transactions.GetByIdAsync(transactionId)).ReturnsAsync(transaction);
        _mapperMock.Setup(m => m.Map<TransactionDto>(transaction)).Returns(expectedDto);

        var query = new GetTransactionByIdQuery(transactionId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        result.Id.Should().Be(transactionId);
        result.Amount.Should().Be(150m);
        _unitOfWorkMock.Verify(u => u.Transactions.GetByIdAsync(transactionId), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingTransaction_ShouldReturnNull()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Transactions.GetByIdAsync(transactionId)).ReturnsAsync((Transaction?)null);

        var query = new GetTransactionByIdQuery(transactionId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _unitOfWorkMock.Verify(u => u.Transactions.GetByIdAsync(transactionId), Times.Once);
        _mapperMock.Verify(m => m.Map<TransactionDto>(It.IsAny<Transaction>()), Times.Never);
    }
}