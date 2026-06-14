using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Categories.Commands.CreateCategory;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Categories.Commands;

public class CreateCategoryCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateCategoryCommandHandler _handler;

    public CreateCategoryCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var categoryRepoMock = new Mock<ICategoryRepository>();
        _unitOfWorkMock.Setup(u => u.Categories).Returns(categoryRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new CreateCategoryCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateCategoryAndReturnDto()
    {
        // Arrange
        var createDto = new CreateCategoryDto { Name = "Transporte", Color = "#00FF00", Icon = "car" };
        var command = new CreateCategoryCommand(createDto);

        var categoryEntity = new Category { Name = "Transporte", Color = "#00FF00", Icon = "car" };
        var expectedDto = new CategoryDto { Id = Guid.NewGuid(), Name = "Transporte", Color = "#00FF00", Icon = "car" };

        _mapperMock.Setup(m => m.Map<Category>(createDto)).Returns(categoryEntity);

        _unitOfWorkMock.Setup(u => u.Categories.AddAsync(It.IsAny<Category>()))
            .ReturnsAsync((Category c) => c);

        _mapperMock.Setup(m => m.Map<CategoryDto>(It.IsAny<Category>())).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        result.Name.Should().Be("Transporte");
        result.Color.Should().Be("#00FF00");

        categoryEntity.Id.Should().NotBeEmpty();
        categoryEntity.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

        _unitOfWorkMock.Verify(u => u.Categories.AddAsync(It.IsAny<Category>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}