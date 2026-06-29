using AutoMapper;
using FluentValidation;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.TimeLogs.Commands.StopTimeLog;

public class StopTimeLogCommandHandler : IRequestHandler<StopTimeLogCommand, TimeLogDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StopTimeLogCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TimeLogDto> Handle(StopTimeLogCommand request, CancellationToken cancellationToken)
    {
        var activeLog = await _unitOfWork.TimeLogs.GetActiveLogForTaskAsync(request.TodoItemId);
        if (activeLog is null)
        {
            throw new ValidationException("No hay ningún registro de tiempo activo para esta tarea.");
        }

        activeLog.EndTime = DateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<TimeLogDto>(activeLog);
    }
}