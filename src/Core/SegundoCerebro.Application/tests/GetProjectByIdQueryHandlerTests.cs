using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Projects.Queries.GetProjectById;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Projects.Queries;

public class GetProjectByIdQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetProjectByIdQueryHandler _handler;

    public GetProjectByIdQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var projectRepoMock = new Mock<IProjectRepository>();
        _unitOfWorkMock.Setup(u => u.Projects).Returns(projectRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new GetProjectByIdQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingProject_ShouldReturnDto()
    {
        var projectId = Guid.NewGuid();
        var project = new Project { Id = projectId, Name = "Proyecto TFG" };
        var expectedDto = new ProjectDto { Id = projectId, Name = "Proyecto TFG" };

        _unitOfWorkMock.Setup(u => u.Projects.GetWithTodoItemsAsync(projectId)).ReturnsAsync(project);
        _mapperMock.Setup(m => m.Map<ProjectDto>(project)).Returns(expectedDto);

        var query = new GetProjectByIdQuery(projectId);
        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        result.Id.Should().Be(projectId);
        result.Name.Should().Be("Proyecto TFG");
        _unitOfWorkMock.Verify(u => u.Projects.GetWithTodoItemsAsync(projectId), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingProject_ShouldReturnNull()
    {
        var projectId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Projects.GetWithTodoItemsAsync(projectId)).ReturnsAsync((Project?)null);

        var query = new GetProjectByIdQuery(projectId);
        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Null(result);
        _unitOfWorkMock.Verify(u => u.Projects.GetWithTodoItemsAsync(projectId), Times.Once);
        _mapperMock.Verify(m => m.Map<ProjectDto>(It.IsAny<Project>()), Times.Never);
    }
}