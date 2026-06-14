using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.TodoItems.Queries.GetTodoItemById;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.TodoItems.Queries;

public class GetTodoItemByIdQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetTodoItemByIdQueryHandler _handler;

    public GetTodoItemByIdQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var todoRepoMock = new Mock<ITodoItemRepository>();
        _unitOfWorkMock.Setup(u => u.TodoItems).Returns(todoRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new GetTodoItemByIdQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingTodoItem_ShouldReturnDto()
    {
        var todoId = Guid.NewGuid();
        var todo = new TodoItem { Id = todoId, Title = "Tarea" };
        var expectedDto = new TodoItemDto { Id = todoId, Title = "Tarea" };

        _unitOfWorkMock.Setup(u => u.TodoItems.GetByIdAsync(todoId)).ReturnsAsync(todo);
        _mapperMock.Setup(m => m.Map<TodoItemDto>(todo)).Returns(expectedDto);

        var query = new GetTodoItemByIdQuery(todoId);
        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        result.Id.Should().Be(todoId);
        result.Title.Should().Be("Tarea");
        _unitOfWorkMock.Verify(u => u.TodoItems.GetByIdAsync(todoId), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingTodoItem_ShouldReturnNull()
    {
        var todoId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.TodoItems.GetByIdAsync(todoId)).ReturnsAsync((TodoItem?)null);

        var query = new GetTodoItemByIdQuery(todoId);
        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Null(result);
        _unitOfWorkMock.Verify(u => u.TodoItems.GetByIdAsync(todoId), Times.Once);
        _mapperMock.Verify(m => m.Map<TodoItemDto>(It.IsAny<TodoItem>()), Times.Never);
    }
}