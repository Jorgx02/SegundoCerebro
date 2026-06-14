using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProjectDto> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var existingProject = await _unitOfWork.Projects.GetByIdAsync(request.Id);
        if (existingProject == null)
            throw new KeyNotFoundException($"Project with ID {request.Id} not found");

        _mapper.Map(request.Project, existingProject);
        existingProject.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Projects.UpdateAsync(existingProject);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ProjectDto>(existingProject);
    }
}
