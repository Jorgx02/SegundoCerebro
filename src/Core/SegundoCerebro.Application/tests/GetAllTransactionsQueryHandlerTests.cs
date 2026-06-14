using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Transactions.Queries.GetAllTransactions;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Transactions.Queries;

public class GetAllTransactionsQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllTransactionsQueryHandler _handler;

    public GetAllTransactionsQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var transactionRepoMock = new Mock<ITransactionRepository>();
        _unitOfWorkMock.Setup(u => u.Transactions).Returns(transactionRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllTransactionsQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfTransactionDtos()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new Transaction { Id = Guid.NewGuid(), Amount = 100m, Description = "Compra 1" },
            new Transaction { Id = Guid.NewGuid(), Amount = 200m, Description = "Compra 2" }
        };

        var transactionDtos = new List<TransactionDto>
        {
            new TransactionDto { Id = transactions[0].Id, Amount = 100m, Description = "Compra 1" },
            new TransactionDto { Id = transactions[1].Id, Amount = 200m, Description = "Compra 2" }
        };

        _unitOfWorkMock.Setup(u => u.Transactions.GetAllAsync()).ReturnsAsync(transactions);
        _mapperMock.Setup(m => m.Map<IEnumerable<TransactionDto>>(transactions)).Returns(transactionDtos);

        var query = new GetAllTransactionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(transactionDtos);
        _unitOfWorkMock.Verify(u => u.Transactions.GetAllAsync(), Times.Once);
    }
}