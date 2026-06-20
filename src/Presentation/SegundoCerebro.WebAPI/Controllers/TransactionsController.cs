using MediatR;
using Microsoft.AspNetCore.Mvc;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Transactions.Commands.CreateTransaction;
using SegundoCerebro.Application.Features.Transactions.Commands.UpdateTransaction;
using SegundoCerebro.Application.Features.Transactions.Commands.DeleteTransaction;
using SegundoCerebro.Application.Features.Transactions.Queries.GetAllTransactions;
using SegundoCerebro.Application.Features.Transactions.Queries.GetTransactionById;

namespace SegundoCerebro.WebAPI.Controllers;

/// <summary>
/// Controlador para gestionar las transacciones financieras.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todas las transacciones del usuario.
    /// </summary>
    /// <returns>Una colección de transacciones.</returns>
    /// <response code="200">Devuelve la lista de transacciones.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAllTransactions()
    {
        var transactions = await _mediator.Send(new GetAllTransactionsQuery());
        return Ok(transactions);
    }

    /// <summary>
    /// Obtiene una transacción específica por su ID.
    /// </summary>
    /// <param name="id">El ID de la transacción a obtener.</param>
    /// <returns>La transacción solicitada.</returns>
    /// <response code="200">Devuelve la transacción encontrada.</response>
    /// <response code="404">Si no se encuentra una transacción con el ID especificado.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionDto>> GetTransaction(Guid id)
    {
        var transaction = await _mediator.Send(new GetTransactionByIdQuery(id));

        if (transaction == null)
            return NotFound();

        return Ok(transaction);
    }

    /// <summary>
    /// Crea una nueva transacción y actualiza el saldo de la cuenta asociada.
    /// </summary>
    /// <param name="createTransactionDto">Los datos para la nueva transacción.</param>
    /// <returns>La transacción recién creada.</returns>
    /// <response code="201">Devuelve la transacción recién creada.</response>
    /// <response code="400">Si los datos de entrada son inválidos o la cuenta no existe.</response>
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

    /// <summary>
    /// Actualiza una transacción existente.
    /// </summary>
    /// <remarks>
    /// Esta operación es compleja: revierte el impacto de la transacción original en el saldo de la cuenta y aplica el nuevo.
    /// </remarks>
    /// <param name="id">El ID de la transacción a actualizar.</param>
    /// <param name="updateTransactionDto">Los nuevos datos para la transacción.</param>
    /// <returns>La transacción actualizada.</returns>
    /// <response code="200">Devuelve la transacción actualizada.</response>
    /// <response code="404">Si no se encuentra una transacción con el ID especificado.</response>
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

    /// <summary>
    /// Elimina una transacción y revierte su impacto en el saldo de la cuenta y el presupuesto asociado.
    /// </summary>
    /// <param name="id">El ID de la transacción a eliminar.</param>
    /// <returns>No devuelve contenido.</returns>
    /// <response code="204">Si la transacción se eliminó correctamente.</response>
    /// <response code="404">Si no se encuentra una transacción con el ID especificado.</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTransaction(Guid id)
    {
        var result = await _mediator.Send(new DeleteTransactionCommand(id));

        if (!result)
            return NotFound();

        return NoContent();
    }
}