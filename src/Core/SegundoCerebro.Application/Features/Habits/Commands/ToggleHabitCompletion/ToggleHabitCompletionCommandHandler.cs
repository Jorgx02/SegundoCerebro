using MediatR;
using SegundoCerebro.Application.Exceptions;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Habits.Commands.ToggleHabitCompletion;

/// <summary>
/// Manejador para el comando que registra o anula el cumplimiento de un hábito.
/// </summary>
public class ToggleHabitCompletionCommandHandler : IRequestHandler<ToggleHabitCompletionCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public ToggleHabitCompletionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Procesa la solicitud de registrar/anular el cumplimiento de un hábito.
    /// </summary>
    /// <param name="request">El comando con los datos del registro.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Verdadero si el hábito quedó como completado, falso en caso contrario.</returns>
    public async Task<bool> Handle(ToggleHabitCompletionCommand request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Habits.ExistsAsync(request.HabitId))
            throw new NotFoundException(nameof(Habit), request.HabitId);

        var existingLog = await _unitOfWork.HabitLogs.GetLogForDateAsync(request.HabitId, request.Date);

        if (existingLog != null)
        {
            await _unitOfWork.HabitLogs.DeleteAsync(existingLog);
            await _unitOfWork.SaveChangesAsync();
            return false; // El hábito ya no está completado para esa fecha
        }

        var newLog = new HabitLog { HabitId = request.HabitId, Date = request.Date.Date };
        await _unitOfWork.HabitLogs.AddAsync(newLog);
        await _unitOfWork.SaveChangesAsync();
        return true; // El hábito ahora está completado para esa fecha
    }
}