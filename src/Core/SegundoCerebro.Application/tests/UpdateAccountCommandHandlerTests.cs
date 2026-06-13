using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Accounts.Commands.UpdateAccount;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Accounts.Commands;

public class UpdateAccountCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateAccountCommandHandler _handler;

    public UpdateAccountCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        // Simulamos un IAccountRepository dentro del UnitOfWork
        var accountRepoMock = new Mock<IAccountRepository>();
        _unitOfWorkMock.Setup(u => u.Accounts).Returns(accountRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new UpdateAccountCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingAccount_ShouldUpdateAndReturnDto()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var updateDto = new UpdateAccountDto { Name = "Nueva Cuenta", Balance = 500m, IsActive = true };
        var command = new UpdateAccountCommand(accountId, updateDto);

        var existingAccount = new Account { Id = accountId, Name = "Vieja Cuenta", Balance = 100m };
        var expectedDto = new AccountDto { Id = accountId, Name = "Nueva Cuenta", Balance = 500m };

        // Configuramos los mocks para que devuelvan nuestros objetos simulados
        _unitOfWorkMock.Setup(u => u.Accounts.GetByIdAsync(accountId)).ReturnsAsync(existingAccount);
        _mapperMock.Setup(m => m.Map<AccountDto>(It.IsAny<Account>())).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        result.Name.Should().Be("Nueva Cuenta");
        result.Balance.Should().Be(500m);

        // Verificamos que se actualizaron los campos obligatorios explícitos
        existingAccount.Balance.Should().Be(500m);
        existingAccount.IsActive.Should().BeTrue();
        existingAccount.UpdatedAt.Should().NotBeNull();

        // Verificamos que se llamó al repositorio y a guardar cambios
        _unitOfWorkMock.Verify(u => u.Accounts.UpdateAsync(existingAccount), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingAccount_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var command = new UpdateAccountCommand(accountId, new UpdateAccountDto());

        // Simulamos que la cuenta no existe en la BD
        _unitOfWorkMock.Setup(u => u.Accounts.GetByIdAsync(accountId)).ReturnsAsync((Account?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Account with ID {accountId} not found");
    }
}
