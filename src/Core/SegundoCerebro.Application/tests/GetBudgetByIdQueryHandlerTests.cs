using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Budgets.Queries.GetBudgetById;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Budgets.Queries;

public class GetBudgetByIdQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetBudgetByIdQueryHandler _handler;

    public GetBudgetByIdQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var budgetRepoMock = new Mock<IBudgetRepository>();
        _unitOfWorkMock.Setup(u => u.Budgets).Returns(budgetRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new GetBudgetByIdQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingBudget_ShouldReturnDto()
    {
        // Arrange
        var budgetId = Guid.NewGuid();
        var budget = new Budget { Id = budgetId, Name = "Alimentación", Amount = 400m };
        var expectedDto = new BudgetDto { Id = budgetId, Name = "Alimentación", Amount = 400m };

        _unitOfWorkMock.Setup(u => u.Budgets.GetByIdAsync(budgetId)).ReturnsAsync(budget);
        _mapperMock.Setup(m => m.Map<BudgetDto>(budget)).Returns(expectedDto);

        var query = new GetBudgetByIdQuery(budgetId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        result.Id.Should().Be(budgetId);
        result.Name.Should().Be("Alimentación");
        result.Amount.Should().Be(400m);
        _unitOfWorkMock.Verify(u => u.Budgets.GetByIdAsync(budgetId), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingBudget_ShouldReturnNull()
    {
        // Arrange
        var budgetId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Budgets.GetByIdAsync(budgetId)).ReturnsAsync((Budget?)null);

        var query = new GetBudgetByIdQuery(budgetId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result); // Usamos Assert.Null por seguridad
        _unitOfWorkMock.Verify(u => u.Budgets.GetByIdAsync(budgetId), Times.Once);
        _mapperMock.Verify(m => m.Map<BudgetDto>(It.IsAny<Budget>()), Times.Never);
    }
}