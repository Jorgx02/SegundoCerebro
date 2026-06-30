using MediatR;

namespace SegundoCerebro.Application.Features.Habits.Commands.DeleteHabit;

/// <summary>
/// Comando para eliminar un hábito por su ID.
/// </summary>
public record DeleteHabitCommand(Guid Id) : IRequest;