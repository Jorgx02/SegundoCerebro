using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Cards.Commands.UpdateCard;
using SegundoCerebro.Application.Features.Cards.Commands.DeleteCard;
using SegundoCerebro.Application.Features.Cards.Commands.CreateCard;
using SegundoCerebro.Application.Features.Cards.Queries.GetCardsByAccount;
using SegundoCerebro.Application.Features.Cards.Queries.GetCardById;

namespace SegundoCerebro.WebAPI.Controllers;

/// <summary>
/// Controlador para gestionar las operaciones relacionadas con las tarjetas (Cards).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CardsController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CardsController"/>.
    /// </summary>
    /// <param name="mediator">Instancia de MediatR para enviar comandos y consultas.</param>
    public CardsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Crea una nueva tarjeta y la asocia a una cuenta existente.
    /// </summary>
    /// <param name="command">Los datos para crear la tarjeta.</param>
    /// <returns>La tarjeta recién creada.</returns>
    /// <response code="201">Devuelve la tarjeta recién creada.</response>
    /// <response code="400">Si los datos de entrada son inválidos.</response>
    /// <response code="404">Si la cuenta especificada no existe.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateCardCommand command)
    {
        var result = await _mediator.Send(command);
        // Devolvemos un 201 Created con la ubicación del recurso (aunque aún no tengamos el GET) y el objeto creado.
        return CreatedAtAction(null, new { id = result.Id }, result);
    }

    /// <summary>
    /// Obtiene una tarjeta específica por su ID.
    /// </summary>
    /// <param name="id">El ID de la tarjeta a obtener.</param>
    /// <returns>Los detalles de la tarjeta.</returns>
    /// <response code="200">Devuelve la tarjeta solicitada.</response>
    /// <response code="404">Si la tarjeta no se encuentra.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CardDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetCardByIdQuery(id);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    /// <summary>
    /// Obtiene todas las tarjetas asociadas a una cuenta específica.
    /// </summary>
    /// <param name="accountId">El ID de la cuenta de la que se quieren obtener las tarjetas.</param>
    /// <returns>Una lista de las tarjetas de la cuenta.</returns>
    /// <response code="200">Devuelve la lista de tarjetas.</response>
    /// <response code="404">Si la cuenta especificada no existe.</response>
    [HttpGet("account/{accountId}")]
    [ProducesResponseType(typeof(IEnumerable<CardDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByAccount(Guid accountId)
    {
        var query = new GetCardsByAccountQuery(accountId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Elimina una tarjeta específica.
    /// </summary>
    /// <param name="id">El ID de la tarjeta a eliminar.</param>
    /// <returns>Una respuesta sin contenido si la eliminación fue exitosa.</returns>
    /// <response code="204">La tarjeta fue eliminada con éxito.</response>
    /// <response code="404">Si la tarjeta no se encuentra.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteCardCommand(id);
        await _mediator.Send(command);

        return NoContent();
    }

    /// <summary>
    /// Actualiza los detalles de una tarjeta existente (ej. nombre, fecha de expiración).
    /// </summary>
    /// <param name="id">El ID de la tarjeta a actualizar.</param>
    /// <param name="command">Los datos a actualizar (nombre, mes/año de expiración).</param>
    /// <returns>La tarjeta actualizada.</returns>
    /// <response code="200">Devuelve la tarjeta actualizada.</response>
    /// <response code="400">Si los datos de entrada son inválidos.</response>
    /// <response code="404">Si la tarjeta no se encuentra.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CardDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCardCommand command)
    {
        // Asignamos el ID de la ruta al comando, que es la fuente de verdad.
        command.Id = id;

        var result = await _mediator.Send(command);
        return Ok(result);
    }
}