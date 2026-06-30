using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Habits.Commands.CreateHabit;

/// <summary>
/// Manejador para el comando de creación de un hábito.
/// </summary>
public class CreateHabitCommandHandler : IRequestHandler<CreateHabitCommand, HabitDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CreateHabitCommandHandler"/>.
    /// </summary>
    public CreateHabitCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Procesa la solicitud de creación de un hábito.
    /// </summary>
    /// <param name="request">El comando con los datos del hábito.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    public async Task<HabitDto> Handle(CreateHabitCommand request, CancellationToken cancellationToken)
    {
        var habit = _mapper.Map<Habit>(request.HabitDto);

        var createdHabit = await _unitOfWork.Habits.AddAsync(habit);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<HabitDto>(createdHabit);
    }
}