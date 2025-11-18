using MediatR;
using Microsoft.AspNetCore.Mvc;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Transactions.Commands.CreateTransaction;
using SegundoCerebro.Application.Features.Transactions.Commands.UpdateTransaction;
using SegundoCerebro.Application.Features.Transactions.Queries.GetAllTransactions;
using SegundoCerebro.Application.Features.Transactions.Queries.GetTransactionById;

namespace SegundoCerebro.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAllTransactions()
    {
        var transactions = await _mediator.Send(new GetAllTransactionsQuery());
        return Ok(transactions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionDto>> GetTransaction(Guid id)
    {
        var transaction = await _mediator.Send(new GetTransactionByIdQuery(id));
        
        if (transaction == null)
            return NotFound();

        return Ok(transaction);
    }

    [HttpPost]
    public async Task<ActionResult<TransactionDto>> CreateTransaction(CreateTransactionDto createTransactionDto)
    {
        try
        {
            var transaction = await _mediator.Send(new CreateTransactionCommand(createTransactionDto));
            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
        }
        catch (KeyNotFoundException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TransactionDto>> UpdateTransaction(Guid id, UpdateTransactionDto updateTransactionDto)
    {
        try
        {
            var transaction = await _mediator.Send(new UpdateTransactionCommand(id, updateTransactionDto));
            return Ok(transaction);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}