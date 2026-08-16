using AutoMapper;
using FluentValidation;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SegundoCerebro.Application.Features.Wellness.Commands.UpsertWellnessEntry;

public class UpsertWellnessEntryCommandHandler : IRequestHandler<UpsertWellnessEntryCommand, WellnessEntryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpsertWellnessEntryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WellnessEntryDto> Handle(UpsertWellnessEntryCommand request, CancellationToken cancellationToken)
    {
        // Validación de seguridad en el backend
        if (request.Dto.Date.Date != DateTime.UtcNow.Date)
        {
            throw new ValidationException("Solo se pueden crear o modificar entradas para el día actual.");
        }

        var existingEntry = (await _unitOfWork.WellnessEntries.FindAsync(e => e.Date.Date == request.Dto.Date.Date)).FirstOrDefault();

        if (existingEntry != null)
        {
            // Actualizar la entrada existente
            existingEntry.MoodRating = request.Dto.MoodRating;
            existingEntry.EnergyLevel = request.Dto.EnergyLevel;
            existingEntry.Notes = request.Dto.Notes;
            await _unitOfWork.WellnessEntries.UpdateAsync(existingEntry);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<WellnessEntryDto>(existingEntry);
        }
        else
        {
            // Crear una nueva entrada
            var newEntry = _mapper.Map<WellnessEntry>(request.Dto);
            var createdEntry = await _unitOfWork.WellnessEntries.AddAsync(newEntry);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<WellnessEntryDto>(createdEntry);
        }
    }
}