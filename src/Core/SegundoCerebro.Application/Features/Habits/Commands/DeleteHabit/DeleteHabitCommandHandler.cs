using MediatR;
using SegundoCerebro.Application.Exceptions;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Habits.Commands.DeleteHabit;

/// <summary>
/// Manejador para el comando de eliminación de un hábito.
/// </summary>
public class DeleteHabitCommandHandler : IRequestHandler<DeleteHabitCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="DeleteHabitCommandHandler"/>.
    /// </summary>
    public DeleteHabitCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Maneja la eliminación de un hábito.
    /// </summary>
    /// <param name="request">El comando que contiene el ID del hábito a eliminar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <exception cref="NotFoundException"></exception>
    public async Task Handle(DeleteHabitCommand request, CancellationToken cancellationToken)
    {
        var habit = await _unitOfWork.Habits.GetByIdAsync(request.Id);
        if (habit is null)
        {
            throw new NotFoundException(nameof(Habit), request.Id);
        }

        await _unitOfWork.Habits.DeleteAsync(habit);
        await _unitOfWork.SaveChangesAsync();
    }
}