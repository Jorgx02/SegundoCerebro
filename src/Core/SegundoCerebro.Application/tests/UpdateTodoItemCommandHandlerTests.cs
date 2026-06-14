using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.TodoItems.Commands.UpdateTodoItem;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.TodoItems.Commands;

public class UpdateTodoItemCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateTodoItemCommandHandler _handler;

    public UpdateTodoItemCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var todoRepoMock = new Mock<ITodoItemRepository>();
        _unitOfWorkMock.Setup(u => u.TodoItems).Returns(todoRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new UpdateTodoItemCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingTodoItem_ShouldUpdateAndSetCompletedAt()
    {
        // Arrange
        var todoId = Guid.NewGuid();
        var updateDto = new UpdateTodoItemDto { Title = "Tarea completada", Status = TodoItemStatus.Completed };
        var command = new UpdateTodoItemCommand(todoId, updateDto);

        var existingTodo = new TodoItem { Id = todoId, Title = "Tarea", Status = TodoItemStatus.Inbox };
        var expectedDto = new TodoItemDto { Id = todoId, Title = "Tarea completada", Status = TodoItemStatus.Completed };

        _unitOfWorkMock.Setup(u => u.TodoItems.GetByIdAsync(todoId)).ReturnsAsync(existingTodo);
        _mapperMock.Setup(m => m.Map(updateDto, existingTodo)).Callback<UpdateTodoItemDto, TodoItem>((src, dest) =>
        {
            dest.Title = src.Title;
            dest.Status = src.Status;
        });
        _mapperMock.Setup(m => m.Map<TodoItemDto>(existingTodo)).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);

        // Verifica la lógica automática de completado
        existingTodo.CompletedAt.Should().NotBeNull();
        existingTodo.UpdatedAt.Should().NotBeNull();

        _unitOfWorkMock.Verify(u => u.TodoItems.UpdateAsync(existingTodo), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingTodoItem_ShouldThrowKeyNotFoundException()
    {
        var todoId = Guid.NewGuid();
        var command = new UpdateTodoItemCommand(todoId, new UpdateTodoItemDto());
        _unitOfWorkMock.Setup(u => u.TodoItems.GetByIdAsync(todoId)).ReturnsAsync((TodoItem?)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}