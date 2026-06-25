using AutoMapper;
using FluentValidation;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Exceptions;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Cards.Commands.CreateCard;

/// <summary>
/// Handler para el comando CreateCardCommand.
/// </summary>
public class CreateCardCommandHandler : IRequestHandler<CreateCardCommand, CardDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CreateCardCommandHandler"/>.
    /// </summary>
    /// <param name="unitOfWork">La unidad de trabajo para acceder a los repositorios.</param>
    /// <param name="mapper">El mapeador de objetos.</param>
    public CreateCardCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Maneja la lógica para crear una nueva tarjeta.
    /// </summary>
    /// <param name="request">El comando con los datos de la tarjeta a crear.</param>
    /// <param name="cancellationToken">El token de cancelación.</param>
    /// <returns>Un DTO con la información de la tarjeta creada.</returns>
    /// <exception cref="NotFoundException">Se lanza si la cuenta especificada no existe.</exception>
    /// <exception cref="ValidationException">Se lanza si se intenta asociar una tarjeta a una cuenta que no es de tipo 'Checking'.</exception>
    public async Task<CardDto> Handle(CreateCardCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar que la cuenta existe
        var account = await _unitOfWork.Accounts.GetByIdAsync(request.AccountId);
        if (account is null)
        {
            throw new NotFoundException(nameof(Account), request.AccountId);
        }

        // 2. Aplicar regla de negocio (ADR 001): Solo cuentas 'Checking' pueden tener tarjetas.
        if (account.Type != AccountType.Checking)
        {
            throw new ValidationException("Las tarjetas solo pueden ser asociadas a cuentas de tipo 'Checking'.");
        }

        // 3. Mapear, añadir y guardar
        var cardEntity = _mapper.Map<Card>(request);
        await _unitOfWork.Cards.AddAsync(cardEntity);
        await _unitOfWork.SaveChangesAsync();

        // 4. Mapear y devolver el resultado
        return _mapper.Map<CardDto>(cardEntity);
    }
}