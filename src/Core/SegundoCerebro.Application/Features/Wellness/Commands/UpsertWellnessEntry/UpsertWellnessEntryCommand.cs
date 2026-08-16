using MediatR;
using SegundoCerebro.Application.DTOs;

namespace SegundoCerebro.Application.Features.Wellness.Commands.UpsertWellnessEntry;

/// <summary>
/// Comando para crear o actualizar una entrada en el diario de bienestar.
/// </summary>
public record UpsertWellnessEntryCommand(UpsertWellnessEntryDto Dto) : IRequest<WellnessEntryDto>;