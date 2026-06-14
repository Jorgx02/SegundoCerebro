using FluentAssertions;
using Moq;
using SegundoCerebro.Application.Features.Projects.Commands.DeleteProject;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Projects.Commands;

public class DeleteProjectCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteProjectCommandHandler _handler;

    public DeleteProjectCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var projectRepoMock = new Mock<IProjectRepository>();
        _unitOfWorkMock.Setup(u => u.Projects).Returns(projectRepoMock.Object);

        _handler = new DeleteProjectCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingProject_ShouldDeleteAndReturnTrue()
    {
        var projectId = Guid.NewGuid();
        var command = new DeleteProjectCommand(projectId);
        var existingProject = new Project { Id = projectId };

        _unitOfWorkMock.Setup(u => u.Projects.GetByIdAsync(projectId)).ReturnsAsync(existingProject);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
        _unitOfWorkMock.Verify(u => u.Projects.DeleteAsync(existingProject), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingProject_ShouldReturnFalse()
    {
        var projectId = Guid.NewGuid();
        var command = new DeleteProjectCommand(projectId);
        _unitOfWorkMock.Setup(u => u.Projects.GetByIdAsync(projectId)).ReturnsAsync((Project?)null);

        var result = await _handler.Handle(command, CancellationToken.None);
        result.Should().BeFalse();
    }
}