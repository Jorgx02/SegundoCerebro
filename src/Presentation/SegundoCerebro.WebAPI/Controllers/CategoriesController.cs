using MediatR;
using Microsoft.AspNetCore.Mvc;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Categories.Commands.CreateCategory;
using SegundoCerebro.Application.Features.Categories.Commands.UpdateCategory;
using SegundoCerebro.Application.Features.Categories.Queries.GetAllCategories;
using SegundoCerebro.Application.Features.Categories.Queries.GetCategoryById;

namespace SegundoCerebro.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
    {
        var categories = await _mediator.Send(new GetAllCategoriesQuery());
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
    {
        var category = await _mediator.Send(new GetCategoryByIdQuery(id));
        
        if (category == null)
            return NotFound();

        return Ok(category);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto createCategoryDto)
    {
        var category = await _mediator.Send(new CreateCategoryCommand(createCategoryDto));
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

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