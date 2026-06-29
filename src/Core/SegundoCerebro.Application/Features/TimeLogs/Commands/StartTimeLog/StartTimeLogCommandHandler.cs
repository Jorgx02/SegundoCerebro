using AutoMapper;
using FluentValidation;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Exceptions;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.TimeLogs.Commands.StartTimeLog;

public class StartTimeLogCommandHandler : IRequestHandler<StartTimeLogCommand, TimeLogDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StartTimeLogCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TimeLogDto> Handle(StartTimeLogCommand request, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.TodoItems.GetByIdAsync(request.TodoItemId) is null)
            throw new NotFoundException(nameof(TodoItem), request.TodoItemId);

        if (await _unitOfWork.TimeLogs.GetActiveLogForTaskAsync(request.TodoItemId) is not null)
            throw new ValidationException("Ya existe un registro de tiempo activo para esta tarea.");

        var newTimeLog = new TimeLog
        {
            TodoItemId = request.TodoItemId,
            StartTime = DateTime.UtcNow
        };

        var createdLog = await _unitOfWork.TimeLogs.AddAsync(newTimeLog);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<TimeLogDto>(createdLog);
    }
}