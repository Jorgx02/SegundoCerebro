using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Accounts.Commands.CreateAccount;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Accounts.Commands;

public class CreateAccountCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateAccountCommandHandler _handler;

    public CreateAccountCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        // Simulamos un IAccountRepository dentro del UnitOfWork
        var accountRepoMock = new Mock<IAccountRepository>();
        _unitOfWorkMock.Setup(u => u.Accounts).Returns(accountRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new CreateAccountCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateAccountAndReturnDto()
    {
        // Arrange
        var createDto = new CreateAccountDto { Name = "Nueva Cuenta Ahorro", InitialBalance = 1500m, Currency = "EUR" };
        var command = new CreateAccountCommand(createDto);

        var accountEntity = new Account { Name = "Nueva Cuenta Ahorro", Balance = 1500m, Currency = "EUR" };
        var expectedDto = new AccountDto { Id = Guid.NewGuid(), Name = "Nueva Cuenta Ahorro", Balance = 1500m, Currency = "EUR" };

        // Configuramos el mock de AutoMapper para Mapear DTO -> Entidad
        _mapperMock.Setup(m => m.Map<Account>(createDto)).Returns(accountEntity);

        // Configuramos el mock del UnitOfWork para devolver la entidad cuando se añada
        _unitOfWorkMock.Setup(u => u.Accounts.AddAsync(It.IsAny<Account>()))
            .ReturnsAsync((Account acc) => acc);

        // Configuramos el mock de AutoMapper para Mapear Entidad -> DTO (el resultado final)
        _mapperMock.Setup(m => m.Map<AccountDto>(It.IsAny<Account>())).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        result.Name.Should().Be("Nueva Cuenta Ahorro");
        result.Balance.Should().Be(1500m);

        // Verificamos que el Handler haya asignado un Id nuevo y la fecha de creación a la entidad
        accountEntity.Id.Should().NotBeEmpty();
        accountEntity.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

        // Verificamos que se llamó al repositorio para añadir y al UnitOfWork para guardar los cambios
        _unitOfWorkMock.Verify(u => u.Accounts.AddAsync(It.IsAny<Account>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}