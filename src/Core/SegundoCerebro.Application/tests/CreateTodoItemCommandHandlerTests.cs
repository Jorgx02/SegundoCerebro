using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.TodoItems.Commands.CreateTodoItem;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.TodoItems.Commands;

public class CreateTodoItemCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateTodoItemCommandHandler _handler;

    public CreateTodoItemCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var todoRepoMock = new Mock<ITodoItemRepository>();
        _unitOfWorkMock.Setup(u => u.TodoItems).Returns(todoRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new CreateTodoItemCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateTodoItemAndReturnDto()
    {
        // Arrange
        var createDto = new CreateTodoItemDto { Title = "Comprar leche" };
        var command = new CreateTodoItemCommand(createDto);

        var todoEntity = new TodoItem { Title = "Comprar leche" };
        var expectedDto = new TodoItemDto { Id = Guid.NewGuid(), Title = "Comprar leche", Status = TodoItemStatus.Inbox };

        _mapperMock.Setup(m => m.Map<TodoItem>(createDto)).Returns(todoEntity);
        _unitOfWorkMock.Setup(u => u.TodoItems.AddAsync(It.IsAny<TodoItem>())).ReturnsAsync((TodoItem t) => t);
        _mapperMock.Setup(m => m.Map<TodoItemDto>(It.IsAny<TodoItem>())).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        result.Title.Should().Be("Comprar leche");

        todoEntity.Id.Should().NotBeEmpty();
        todoEntity.Status.Should().Be(TodoItemStatus.Inbox);
        todoEntity.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

        _unitOfWorkMock.Verify(u => u.TodoItems.AddAsync(It.IsAny<TodoItem>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}