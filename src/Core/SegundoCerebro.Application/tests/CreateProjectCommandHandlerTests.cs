using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Projects.Commands.CreateProject;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Projects.Commands;

public class CreateProjectCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateProjectCommandHandler _handler;

    public CreateProjectCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var projectRepoMock = new Mock<IProjectRepository>();
        _unitOfWorkMock.Setup(u => u.Projects).Returns(projectRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new CreateProjectCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateProjectAndReturnDto()
    {
        // Arrange
        var createDto = new CreateProjectDto { Name = "Proyecto TFG" };
        var command = new CreateProjectCommand(createDto);

        var projectEntity = new Project { Name = "Proyecto TFG" };
        var expectedDto = new ProjectDto { Id = Guid.NewGuid(), Name = "Proyecto TFG", Status = ProjectStatus.NotStarted };

        _mapperMock.Setup(m => m.Map<Project>(createDto)).Returns(projectEntity);
        _unitOfWorkMock.Setup(u => u.Projects.AddAsync(It.IsAny<Project>())).ReturnsAsync((Project p) => p);
        _mapperMock.Setup(m => m.Map<ProjectDto>(It.IsAny<Project>())).Returns(expectedDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        result.Name.Should().Be("Proyecto TFG");

        projectEntity.Id.Should().NotBeEmpty();
        projectEntity.Status.Should().Be(ProjectStatus.NotStarted);
        projectEntity.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

        _unitOfWorkMock.Verify(u => u.Projects.AddAsync(It.IsAny<Project>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}