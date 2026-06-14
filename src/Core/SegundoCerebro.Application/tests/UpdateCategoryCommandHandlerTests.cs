using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Categories.Commands.UpdateCategory;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Categories.Commands;

public class UpdateCategoryCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateCategoryCommandHandler _handler;

    public UpdateCategoryCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var categoryRepoMock = new Mock<ICategoryRepository>();
        _unitOfWorkMock.Setup(u => u.Categories).Returns(categoryRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new UpdateCategoryCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingCategory_ShouldUpdateAndReturnDto()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var updateDto = new UpdateCategoryDto { Name = "Transporte Plus", Color = "#FFFFFF" };
        var command = new UpdateCategoryCommand(categoryId, updateDto);

        var existingCategory = new Category { Id = categoryId, Name = "Transporte", Color = "#000000" };
        var expectedDto = new CategoryDto { Id = categoryId, Name = "Transporte Plus", Color = "#FFFFFF" };

        _unitOfWorkMock.Setup(u => u.Categories.GetByIdAsync(categoryId)).ReturnsAsync(existingCategory);

        _mapperMock.Setup(m => m.Map(updateDto, existingCategory)).Callback<UpdateCategoryDto, Category>((src, dest) =>
        {
            dest.Name = src.Name;
            dest.Color = src.Color;
        });

        _mapperMock.Setup(m => m.Map<CategoryDto>(existingCategory)).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        result.Name.Should().Be("Transporte Plus");

        existingCategory.Name.Should().Be("Transporte Plus");
        existingCategory.UpdatedAt.Should().NotBeNull();

        _unitOfWorkMock.Verify(u => u.Categories.UpdateAsync(existingCategory), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingCategory_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new UpdateCategoryCommand(categoryId, new UpdateCategoryDto());
        _unitOfWorkMock.Setup(u => u.Categories.GetByIdAsync(categoryId)).ReturnsAsync((Category?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}