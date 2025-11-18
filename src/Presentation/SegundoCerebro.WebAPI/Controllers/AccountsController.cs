using MediatR;
using Microsoft.AspNetCore.Mvc;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Accounts.Commands.CreateAccount;
using SegundoCerebro.Application.Features.Accounts.Commands.DeleteAccount;
using SegundoCerebro.Application.Features.Accounts.Commands.UpdateAccount;
using SegundoCerebro.Application.Features.Accounts.Queries.GetAccountById;
using SegundoCerebro.Application.Features.Accounts.Queries.GetAllAccounts;

namespace SegundoCerebro.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAllAccounts()
    {
        var accounts = await _mediator.Send(new GetAllAccountsQuery());
        return Ok(accounts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountDto>> GetAccount(Guid id)
    {
        var account = await _mediator.Send(new GetAccountByIdQuery(id));
        
        if (account == null)
            return NotFound();

        return Ok(account);
    }

    [HttpPost]
    public async Task<ActionResult<AccountDto>> CreateAccount(CreateAccountDto createAccountDto)
    {
        var account = await _mediator.Send(new CreateAccountCommand(createAccountDto));
        return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AccountDto>> UpdateAccount(Guid id, UpdateAccountDto updateAccountDto)
    {
        try
        {
            var account = await _mediator.Send(new UpdateAccountCommand(id, updateAccountDto));
            return Ok(account);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAccount(Guid id)
    {
        var result = await _mediator.Send(new DeleteAccountCommand(id));
        
        if (!result)
            return NotFound();

        return NoContent();
    }
}