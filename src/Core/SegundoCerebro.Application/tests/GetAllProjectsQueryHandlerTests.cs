using AutoMapper;
using FluentAssertions;
using Moq;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Projects.Queries.GetAllProjects;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using Xunit;

namespace SegundoCerebro.Application.UnitTests.Features.Projects.Queries;

public class GetAllProjectsQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllProjectsQueryHandler _handler;

    public GetAllProjectsQueryHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var projectRepoMock = new Mock<IProjectRepository>();
        _unitOfWorkMock.Setup(u => u.Projects).Returns(projectRepoMock.Object);

        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllProjectsQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnListOfProjectDtos()
    {
        var projects = new List<Project>
        {
            new Project { Id = Guid.NewGuid(), Name = "Proyecto 1" },
            new Project { Id = Guid.NewGuid(), Name = "Proyecto 2" }
        };

        var projectDtos = new List<ProjectDto>
        {
            new ProjectDto { Id = projects[0].Id, Name = "Proyecto 1" },
            new ProjectDto { Id = projects[1].Id, Name = "Proyecto 2" }
        };

        _unitOfWorkMock.Setup(u => u.Projects.GetActiveProjectsAsync()).ReturnsAsync(projects);
        _mapperMock.Setup(m => m.Map<IEnumerable<ProjectDto>>(projects)).Returns(projectDtos);

        var query = new GetAllProjectsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(projectDtos);
        _unitOfWorkMock.Verify(u => u.Projects.GetActiveProjectsAsync(), Times.Once);
    }
}