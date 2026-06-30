using MediatR;
using System.Text.Json.Serialization;

namespace SegundoCerebro.Application.Features.Habits.Commands.ToggleHabitCompletion;

/// <summary>
/// Comando para registrar o anular el cumplimiento de un hábito en una fecha específica.
/// </summary>
public class ToggleHabitCompletionCommand : IRequest<bool>
{
    [JsonIgnore]
    public Guid HabitId { get; set; }

    /// <summary>La fecha para la cual se registra el cumplimiento.</summary>
    public DateTime Date { get; set; }
}