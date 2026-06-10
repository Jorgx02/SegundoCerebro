using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Accounts.Commands.UpdateAccount;

/// <summary>
/// Manejador del comando para actualizar una cuenta financiera existente.
/// Implementa CQRS a través de MediatR para separar la lógica de escritura.
/// </summary>
public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, AccountDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor que inyecta las dependencias necesarias.
    /// </summary>
    public UpdateAccountCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Ejecuta la lógica de actualización de la cuenta en la base de datos de forma segura.
    /// </summary>
    public async Task<AccountDto> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var existingAccount = await _unitOfWork.Accounts.GetByIdAsync(request.Id);
        if (existingAccount == null)
            throw new KeyNotFoundException($"Account with ID {request.Id} not found");

        _mapper.Map(request.Account, existingAccount);

        // Asignación explícita para evitar que AutoMapper lo ignore por seguridad
        existingAccount.Balance = request.Account.Balance;
        existingAccount.IsActive = request.Account.IsActive;

        existingAccount.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Accounts.UpdateAsync(existingAccount);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<AccountDto>(existingAccount);
    }
}