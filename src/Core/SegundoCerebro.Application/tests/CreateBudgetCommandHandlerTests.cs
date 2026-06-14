using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Budgets.Commands.CreateBudget;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Budgets.Commands;

public class CreateBudgetCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateBudgetCommandHandler _handler;

    public CreateBudgetCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        // Simulamos un IBudgetRepository dentro del UnitOfWork
        var budgetRepoMock = new Mock<IBudgetRepository>();
        _unitOfWorkMock.Setup(u => u.Budgets).Returns(budgetRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new CreateBudgetCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateBudgetAndReturnDto()
    {
        // Arrange
        var createDto = new CreateBudgetDto { Name = "Supermercado", Amount = 400m };
        var command = new CreateBudgetCommand(createDto);

        var budgetEntity = new Budget { Name = "Supermercado", Amount = 400m };
        var expectedDto = new BudgetDto { Id = Guid.NewGuid(), Name = "Supermercado", Amount = 400m };

        // Mocks de AutoMapper y UnitOfWork
        _mapperMock.Setup(m => m.Map<Budget>(createDto)).Returns(budgetEntity);

        _unitOfWorkMock.Setup(u => u.Budgets.AddAsync(It.IsAny<Budget>()))
            .ReturnsAsync((Budget b) => b);

        _mapperMock.Setup(m => m.Map<BudgetDto>(It.IsAny<Budget>())).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        result.Name.Should().Be("Supermercado");
        result.Amount.Should().Be(400m);

        budgetEntity.Id.Should().NotBeEmpty();
        budgetEntity.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

        _unitOfWorkMock.Verify(u => u.Budgets.AddAsync(It.IsAny<Budget>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}