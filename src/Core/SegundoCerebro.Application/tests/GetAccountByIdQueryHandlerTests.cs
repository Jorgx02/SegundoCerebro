using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Accounts.Queries.GetAccountById;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Accounts.Queries;

public class GetAccountByIdQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAccountByIdQueryHandler _handler;

    public GetAccountByIdQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var accountRepoMock = new Mock<IAccountRepository>();
        _unitOfWorkMock.Setup(u => u.Accounts).Returns(accountRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new GetAccountByIdQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingAccount_ShouldReturnDto()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var account = new Account { Id = accountId, Name = "Cuenta Principal", Balance = 2000m };
        var expectedDto = new AccountDto { Id = accountId, Name = "Cuenta Principal", Balance = 2000m };

        _unitOfWorkMock.Setup(u => u.Accounts.GetByIdAsync(accountId)).ReturnsAsync(account);
        _mapperMock.Setup(m => m.Map<AccountDto>(account)).Returns(expectedDto);

        var query = new GetAccountByIdQuery(accountId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        result.Id.Should().Be(accountId);
        result.Name.Should().Be("Cuenta Principal");
        result.Balance.Should().Be(2000m);
        _unitOfWorkMock.Verify(u => u.Accounts.GetByIdAsync(accountId), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingAccount_ShouldReturnNull()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Accounts.GetByIdAsync(accountId)).ReturnsAsync((Account?)null);

        var query = new GetAccountByIdQuery(accountId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result); // Usamos Assert.Null por la misma razón que en el anterior para evitar fallos del Enum
        _unitOfWorkMock.Verify(u => u.Accounts.GetByIdAsync(accountId), Times.Once);
        _mapperMock.Verify(m => m.Map<AccountDto>(It.IsAny<Account>()), Times.Never);
    }
}