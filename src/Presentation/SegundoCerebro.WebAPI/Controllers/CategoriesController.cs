using MediatR;
using Microsoft.AspNetCore.Mvc;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Categories.Commands.CreateCategory;
using SegundoCerebro.Application.Features.Categories.Commands.UpdateCategory;
using SegundoCerebro.Application.Features.Categories.Queries.GetAllCategories;
using SegundoCerebro.Application.Features.Categories.Queries.GetCategoryById;
// Asumiendo que existe un comando para eliminar, similar a los otros controladores.
// using SegundoCerebro.Application.Features.Categories.Commands.DeleteCategory;

namespace SegundoCerebro.WebAPI.Controllers;

/// <summary>
/// Controlador para la gestión de categorías de transacciones.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todas las categorías activas del usuario.
    /// </summary>
    /// <returns>Una colección de categorías.</returns>
    /// <response code="200">Devuelve la lista de categorías.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
    {
        var categories = await _mediator.Send(new GetAllCategoriesQuery());
        return Ok(categories);
    }

    /// <summary>
    /// Obtiene una categoría específica por su ID.
    /// </summary>
    /// <param name="id">El ID de la categoría a obtener.</param>
    /// <returns>La categoría solicitada.</returns>
    /// <response code="200">Devuelve la categoría encontrada.</response>
    /// <response code="404">Si no se encuentra una categoría con el ID especificado.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
    {
        var category = await _mediator.Send(new GetCategoryByIdQuery(id));

        if (category == null)
            return NotFound();

        return Ok(category);
    }

    /// <summary>
    /// Crea una nueva categoría.
    /// </summary>
    /// <param name="createCategoryDto">Los datos para la nueva categoría.</param>
    /// <returns>La categoría recién creada.</returns>
    /// <response code="201">Devuelve la categoría recién creada.</response>
    /// <response code="400">Si los datos de entrada no son válidos.</response>
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto createCategoryDto)
    {
        var category = await _mediator.Send(new CreateCategoryCommand(createCategoryDto));
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    /// <summary>
    /// Actualiza una categoría existente.
    /// </summary>
    /// <param name="id">El ID de la categoría a actualizar.</param>
    /// <param name="updateCategoryDto">Los nuevos datos para la categoría.</param>
    /// <returns>La categoría actualizada.</returns>
    /// <response code="200">Devuelve la categoría actualizada.</response>
    /// <response code="404">Si no se encuentra una categoría con el ID especificado.</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryDto>> UpdateCategory(Guid id, UpdateCategoryDto updateCategoryDto)
    {
        try
        {
            var category = await _mediator.Send(new UpdateCategoryCommand(id, updateCategoryDto));
            return Ok(category);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}