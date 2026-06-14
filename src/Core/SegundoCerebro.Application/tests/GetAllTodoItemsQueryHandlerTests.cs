using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.TodoItems.Queries.GetAllTodoItems;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.TodoItems.Queries;

public class GetAllTodoItemsQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllTodoItemsQueryHandler _handler;

    public GetAllTodoItemsQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var todoRepoMock = new Mock<ITodoItemRepository>();
        _unitOfWorkMock.Setup(u => u.TodoItems).Returns(todoRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllTodoItemsQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfTodoItemDtos()
    {
        var todos = new List<TodoItem>
        {
            new TodoItem { Id = Guid.NewGuid(), Title = "Tarea 1" },
            new TodoItem { Id = Guid.NewGuid(), Title = "Tarea 2" }
        };

        var todoDtos = new List<TodoItemDto>
        {
            new TodoItemDto { Id = todos[0].Id, Title = "Tarea 1" },
            new TodoItemDto { Id = todos[1].Id, Title = "Tarea 2" }
        };

        _unitOfWorkMock.Setup(u => u.TodoItems.GetAllAsync()).ReturnsAsync(todos);
        _mapperMock.Setup(m => m.Map<IEnumerable<TodoItemDto>>(todos)).Returns(todoDtos);

        var query = new GetAllTodoItemsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(todoDtos);
        _unitOfWorkMock.Verify(u => u.TodoItems.GetAllAsync(), Times.Once);
    }
}