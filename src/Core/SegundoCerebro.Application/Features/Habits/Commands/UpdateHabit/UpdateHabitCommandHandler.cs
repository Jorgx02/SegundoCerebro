using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Exceptions;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Habits.Commands.UpdateHabit;

/// <summary>
/// Manejador para el comando de actualización de un hábito.
/// </summary>
public class UpdateHabitCommandHandler : IRequestHandler<UpdateHabitCommand, HabitDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UpdateHabitCommandHandler"/>.
    /// </summary>
    public UpdateHabitCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Actualiza un hábito existente.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="NotFoundException"></exception>
    public async Task<HabitDto> Handle(UpdateHabitCommand request, CancellationToken cancellationToken)
    {
        var habitToUpdate = await _unitOfWork.Habits.GetByIdAsync(request.Id);
        if (habitToUpdate is null)
        {
            throw new NotFoundException(nameof(Habit), request.Id);
        }

        _mapper.Map(request.HabitDto, habitToUpdate);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<HabitDto>(habitToUpdate);
    }
}