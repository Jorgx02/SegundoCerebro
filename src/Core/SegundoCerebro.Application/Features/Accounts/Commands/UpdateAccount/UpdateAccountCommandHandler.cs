using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Accounts.Commands.UpdateAccount;

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, AccountDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateAccountCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AccountDto> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var existingAccount = await _unitOfWork.Accounts.GetByIdAsync(request.Id);
        if (existingAccount == null)
            throw new KeyNotFoundException($"Account with ID {request.Id} not found");

        _mapper.Map(request.Account, existingAccount);
        existingAccount.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Accounts.UpdateAsync(existingAccount);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<AccountDto>(existingAccount);
    }
}