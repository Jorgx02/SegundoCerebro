using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Accounts.Commands.CreateAccount;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, AccountDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAccountCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AccountDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = _mapper.Map<Account>(request.Account);
        account.Id = Guid.NewGuid();
        account.CreatedAt = DateTime.UtcNow;

        var createdAccount = await _unitOfWork.Accounts.AddAsync(account);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<AccountDto>(createdAccount);
    }
}