using FluentAssertions;
using Moq;
using SegundoCerebro.Application.Features.TodoItems.Commands.DeleteTodoItem;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.TodoItems.Commands;

public class DeleteTodoItemCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteTodoItemCommandHandler _handler;

    public DeleteTodoItemCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var todoRepoMock = new Mock<ITodoItemRepository>();
        _unitOfWorkMock.Setup(u => u.TodoItems).Returns(todoRepoMock.Object);

        _handler = new DeleteTodoItemCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingTodoItem_ShouldDeleteAndReturnTrue()
    {
        var todoId = Guid.NewGuid();
        var command = new DeleteTodoItemCommand(todoId);
        var existingTodo = new TodoItem { Id = todoId };

        _unitOfWorkMock.Setup(u => u.TodoItems.GetByIdAsync(todoId)).ReturnsAsync(existingTodo);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
        _unitOfWorkMock.Verify(u => u.TodoItems.DeleteAsync(existingTodo), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingTodoItem_ShouldReturnFalse()
    {
        var todoId = Guid.NewGuid();
        var command = new DeleteTodoItemCommand(todoId);
        _unitOfWorkMock.Setup(u => u.TodoItems.GetByIdAsync(todoId)).ReturnsAsync((TodoItem?)null);

        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().BeFalse();
    }
}