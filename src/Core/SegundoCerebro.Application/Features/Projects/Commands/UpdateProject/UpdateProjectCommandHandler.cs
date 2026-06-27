using AutoMapper;
using FluentValidation;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Exceptions;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;
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
        var projectToUpdate = await _unitOfWork.Projects.GetWithTodoItemsAsync(request.Id);
        if (projectToUpdate is null)
        {
            throw new NotFoundException(nameof(Project), request.Id);
        }

        // Regla de negocio: No se puede completar un proyecto si tiene tareas pendientes.
        if (request.ProjectDto.Status == ProjectStatus.Completed && projectToUpdate.TodoItems.Any(t => t.Status != TodoItemStatus.Completed))
        {
            throw new ValidationException("No se puede completar el proyecto. Aún existen tareas pendientes.");
        }

        // Mapear los nuevos valores desde el DTO a la entidad existente
        _mapper.Map(request.ProjectDto, projectToUpdate);
        projectToUpdate.UpdatedAt = DateTime.UtcNow;

        // No es necesario llamar a UpdateAsync porque el objeto ya está siendo rastreado por EF Core
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ProjectDto>(projectToUpdate);
    }
}