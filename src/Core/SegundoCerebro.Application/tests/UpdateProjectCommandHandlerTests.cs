using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Projects.Commands.UpdateProject;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Projects.Commands;

public class UpdateProjectCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateProjectCommandHandler _handler;

    public UpdateProjectCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var projectRepoMock = new Mock<IProjectRepository>();
        _unitOfWorkMock.Setup(u => u.Projects).Returns(projectRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new UpdateProjectCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingProject_ShouldUpdateAndReturnDto()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var updateDto = new UpdateProjectDto { Name = "TFG Activo", Status = ProjectStatus.Active };
        var command = new UpdateProjectCommand(projectId, updateDto);

        var existingProject = new Project { Id = projectId, Name = "TFG", Status = ProjectStatus.NotStarted };
        var expectedDto = new ProjectDto { Id = projectId, Name = "TFG Activo", Status = ProjectStatus.Active };

        _unitOfWorkMock.Setup(u => u.Projects.GetByIdAsync(projectId)).ReturnsAsync(existingProject);

        _mapperMock.Setup(m => m.Map(updateDto, existingProject)).Callback<UpdateProjectDto, Project>((src, dest) =>
        {
            dest.Name = src.Name;
            dest.Status = src.Status;
        });

        _mapperMock.Setup(m => m.Map<ProjectDto>(existingProject)).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        existingProject.Name.Should().Be("TFG Activo");
        existingProject.UpdatedAt.Should().NotBeNull();

        _unitOfWorkMock.Verify(u => u.Projects.UpdateAsync(existingProject), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingProject_ShouldThrowKeyNotFoundException()
    {
        var projectId = Guid.NewGuid();
        var command = new UpdateProjectCommand(projectId, new UpdateProjectDto());
        _unitOfWorkMock.Setup(u => u.Projects.GetByIdAsync(projectId)).ReturnsAsync((Project?)null);
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}