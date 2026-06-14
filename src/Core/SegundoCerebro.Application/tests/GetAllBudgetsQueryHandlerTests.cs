using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Budgets.Queries.GetAllBudgets;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Budgets.Queries;

public class GetAllBudgetsQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllBudgetsQueryHandler _handler;

    public GetAllBudgetsQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var budgetRepoMock = new Mock<IBudgetRepository>();
        _unitOfWorkMock.Setup(u => u.Budgets).Returns(budgetRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllBudgetsQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfBudgetDtos()
    {
        // Arrange
        var budgets = new List<Budget>
        {
            new Budget { Id = Guid.NewGuid(), Name = "Alimentación", Amount = 300m },
            new Budget { Id = Guid.NewGuid(), Name = "Ocio", Amount = 150m }
        };

        var budgetDtos = new List<BudgetDto>
        {
            new BudgetDto { Id = budgets[0].Id, Name = "Alimentación", Amount = 300m },
            new BudgetDto { Id = budgets[1].Id, Name = "Ocio", Amount = 150m }
        };

        _unitOfWorkMock.Setup(u => u.Budgets.GetActiveBudgetsAsync()).ReturnsAsync(budgets);
        _mapperMock.Setup(m => m.Map<IEnumerable<BudgetDto>>(budgets)).Returns(budgetDtos);

        var query = new GetAllBudgetsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(budgetDtos);
        _unitOfWorkMock.Verify(u => u.Budgets.GetActiveBudgetsAsync(), Times.Once);
    }
}