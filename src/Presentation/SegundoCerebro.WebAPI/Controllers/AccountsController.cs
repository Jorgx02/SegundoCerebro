using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Accounts.Commands.CreateAccount;
using SegundoCerebro.Application.Features.Accounts.Commands.DeleteAccount;
using SegundoCerebro.Application.Features.Accounts.Commands.ToggleFavorite;
using SegundoCerebro.Application.Features.Accounts.Commands.UpdateAccount;
using SegundoCerebro.Application.Features.Accounts.Queries.GetAccountById;
using SegundoCerebro.Application.Features.Accounts.Queries.GetAllAccounts;

namespace SegundoCerebro.WebAPI.Controllers;

/// <summary>
/// Controlador para gestionar las cuentas financieras de los usuarios.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todas las cuentas financieras activas del usuario.
    /// </summary>
    /// <returns>Una colección de cuentas financieras.</returns>
    /// <response code="200">Devuelve la lista de cuentas.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAllAccounts()
    {
        var accounts = await _mediator.Send(new GetAllAccountsQuery());
        return Ok(accounts);
    }

    /// <summary>
    /// Obtiene una cuenta financiera específica por su ID.
    /// </summary>
    /// <param name="id">El ID de la cuenta a obtener.</param>
    /// <returns>La cuenta financiera solicitada.</returns>
    /// <response code="200">Devuelve la cuenta encontrada.</response>
    /// <response code="404">Si no se encuentra una cuenta con el ID especificado.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<AccountDto>> GetAccount(Guid id)
    {
        var account = await _mediator.Send(new GetAccountByIdQuery(id));

        if (account == null)
            return NotFound();

        return Ok(account);
    }

    /// <summary>
    /// Crea una nueva cuenta financiera.
    /// </summary>
    /// <param name="createAccountDto">Los datos para la nueva cuenta.</param>
    /// <returns>La cuenta recién creada.</returns>
    /// <response code="201">Devuelve la cuenta recién creada y la URL para acceder a ella.</response>
    /// <response code="400">Si los datos de entrada no son válidos.</response>
    [HttpPost]
    public async Task<ActionResult<AccountDto>> CreateAccount(CreateAccountDto createAccountDto)
    {
        var account = await _mediator.Send(new CreateAccountCommand(createAccountDto));
        return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
    }

    /// <summary>
    /// Actualiza una cuenta financiera existente.
    /// </summary>
    /// <param name="id">El ID de la cuenta a actualizar.</param>
    /// <param name="updateAccountDto">Los nuevos datos para la cuenta.</param>
    /// <returns>La cuenta actualizada.</returns>
    /// <response code="200">Devuelve la cuenta actualizada.</response>
    /// <response code="400">Si los datos de entrada no son válidos.</response>
    /// <response code="404">Si no se encuentra una cuenta con el ID especificado.</response>
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

    /// <summary>
    /// Desactiva una cuenta financiera (Soft Delete).
    /// </summary>
    /// <param name="id">El ID de la cuenta a desactivar.</param>
    /// <returns>No devuelve contenido.</returns>
    /// <response code="204">Si la cuenta se desactivó correctamente.</response>
    /// <response code="404">Si no se encuentra una cuenta con el ID especificado.</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAccount(Guid id)
    {
        var result = await _mediator.Send(new DeleteAccountCommand(id));

        if (!result)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Cambia el estado de 'favorito' de una cuenta.
    /// </summary>
    /// <param name="id">El ID de la cuenta a modificar.</param>
    /// <returns>No devuelve contenido.</returns>
    /// <response code="204">Si el estado se cambió correctamente.</response>
    /// <response code="404">Si no se encuentra una cuenta con el ID especificado.</response>
    [HttpPatch("{id}/toggle-favorite")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleFavorite(Guid id)
    {
        await _mediator.Send(new ToggleFavoriteAccountCommand(id));
        return NoContent();
    }
}