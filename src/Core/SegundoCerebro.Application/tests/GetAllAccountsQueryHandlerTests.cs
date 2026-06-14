using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Accounts.Queries.GetAllAccounts;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Accounts.Queries;

public class GetAllAccountsQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllAccountsQueryHandler _handler;

    public GetAllAccountsQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var accountRepoMock = new Mock<IAccountRepository>();
        _unitOfWorkMock.Setup(u => u.Accounts).Returns(accountRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllAccountsQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfAccountDtos()
    {
        // Arrange
        var accounts = new List<Account>
        {
            new Account { Id = Guid.NewGuid(), Name = "Cuenta Principal", Balance = 1000m },
            new Account { Id = Guid.NewGuid(), Name = "Ahorros", Balance = 5000m }
        };

        var accountDtos = new List<AccountDto>
        {
            new AccountDto { Id = accounts[0].Id, Name = "Cuenta Principal", Balance = 1000m },
            new AccountDto { Id = accounts[1].Id, Name = "Ahorros", Balance = 5000m }
        };

        _unitOfWorkMock.Setup(u => u.Accounts.GetActiveAccountsAsync()).ReturnsAsync(accounts);
        _mapperMock.Setup(m => m.Map<IEnumerable<AccountDto>>(accounts)).Returns(accountDtos);

        var query = new GetAllAccountsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(accountDtos);
        _unitOfWorkMock.Verify(u => u.Accounts.GetActiveAccountsAsync(), Times.Once);
    }
}