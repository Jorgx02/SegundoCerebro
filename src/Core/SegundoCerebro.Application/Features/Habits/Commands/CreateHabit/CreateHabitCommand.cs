using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Habits.Commands.CreateHabit;

/// <summary>
/// Comando para crear un nuevo hábito.
/// </summary>
public class CreateHabitCommand : IRequest<HabitDto>
{
    /// <summary>
    /// DTO con los datos del hábito a crear.
    /// </summary>
    public CreateHabitDto HabitDto { get; set; } = null!;
}