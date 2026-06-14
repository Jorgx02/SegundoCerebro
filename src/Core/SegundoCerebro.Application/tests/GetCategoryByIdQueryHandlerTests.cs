using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Categories.Queries.GetCategoryById;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Categories.Queries;

public class GetCategoryByIdQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetCategoryByIdQueryHandler _handler;

    public GetCategoryByIdQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var categoryRepoMock = new Mock<ICategoryRepository>();
        _unitOfWorkMock.Setup(u => u.Categories).Returns(categoryRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new GetCategoryByIdQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingCategory_ShouldReturnDto()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var category = new Category { Id = categoryId, Name = "Alimentación", Color = "#FF0000" };
        var expectedDto = new CategoryDto { Id = categoryId, Name = "Alimentación", Color = "#FF0000" };

        _unitOfWorkMock.Setup(u => u.Categories.GetByIdAsync(categoryId)).ReturnsAsync(category);
        _mapperMock.Setup(m => m.Map<CategoryDto>(category)).Returns(expectedDto);

        var query = new GetCategoryByIdQuery(categoryId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        result.Id.Should().Be(categoryId);
        result.Name.Should().Be("Alimentación");
        _unitOfWorkMock.Verify(u => u.Categories.GetByIdAsync(categoryId), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingCategory_ShouldReturnNull()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Categories.GetByIdAsync(categoryId)).ReturnsAsync((Category?)null);

        var query = new GetCategoryByIdQuery(categoryId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result); // Usamos Assert.Null por seguridad para evitar conflictos con FluentAssertions
        _unitOfWorkMock.Verify(u => u.Categories.GetByIdAsync(categoryId), Times.Once);
        _mapperMock.Verify(m => m.Map<CategoryDto>(It.IsAny<Category>()), Times.Never);
    }
}