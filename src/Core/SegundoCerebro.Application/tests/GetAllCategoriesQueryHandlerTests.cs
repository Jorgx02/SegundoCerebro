using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Categories.Queries.GetAllCategories;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Categories.Queries;

public class GetAllCategoriesQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllCategoriesQueryHandler _handler;

    public GetAllCategoriesQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var categoryRepoMock = new Mock<ICategoryRepository>();
        _unitOfWorkMock.Setup(u => u.Categories).Returns(categoryRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllCategoriesQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfCategoryDtos()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category { Id = Guid.NewGuid(), Name = "Alimentación", Color = "#FF0000" },
            new Category { Id = Guid.NewGuid(), Name = "Ocio", Color = "#00FF00" }
        };

        var categoryDtos = new List<CategoryDto>
        {
            new CategoryDto { Id = categories[0].Id, Name = "Alimentación", Color = "#FF0000" },
            new CategoryDto { Id = categories[1].Id, Name = "Ocio", Color = "#00FF00" }
        };

        _unitOfWorkMock.Setup(u => u.Categories.GetActiveCategoriesAsync()).ReturnsAsync(categories);
        _mapperMock.Setup(m => m.Map<IEnumerable<CategoryDto>>(categories)).Returns(categoryDtos);

        var query = new GetAllCategoriesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(categoryDtos);
        _unitOfWorkMock.Verify(u => u.Categories.GetActiveCategoriesAsync(), Times.Once);
    }
}