using MediatR;
using SegundoCerebro.Application.DTOs;
using System.Text.Json.Serialization;

namespace SegundoCerebro.Application.Features.Habits.Commands.UpdateHabit;

/// <summary>
/// Comando para actualizar un hábito existente.
/// </summary>
public class UpdateHabitCommand : IRequest<HabitDto>
{
    [JsonIgnore]
    public Guid Id { get; set; }

    public UpdateHabitDto HabitDto { get; set; } = null!;
}