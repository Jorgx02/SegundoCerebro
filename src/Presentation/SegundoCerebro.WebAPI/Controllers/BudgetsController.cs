using MediatR;
using Microsoft.AspNetCore.Mvc;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Features.Budgets.Commands.CreateBudget;
using SegundoCerebro.Application.Features.Budgets.Commands.UpdateBudget;
using SegundoCerebro.Application.Features.Budgets.Queries.GetAllBudgets;
using SegundoCerebro.Application.Features.Budgets.Queries.GetBudgetById;

namespace SegundoCerebro.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BudgetsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BudgetsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BudgetDto>>> GetAllBudgets()
    {
        var budgets = await _mediator.Send(new GetAllBudgetsQuery());
        return Ok(budgets);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BudgetDto>> GetBudget(Guid id)
    {
        var budget = await _mediator.Send(new GetBudgetByIdQuery(id));
        
        if (budget == null)
            return NotFound();

        return Ok(budget);
    }

    [HttpPost]
    public async Task<ActionResult<BudgetDto>> CreateBudget(CreateBudgetDto createBudgetDto)
    {
        var budget = await _mediator.Send(new CreateBudgetCommand(createBudgetDto));
        return CreatedAtAction(nameof(GetBudget), new { id = budget.Id }, budget);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BudgetDto>> UpdateBudget(Guid id, UpdateBudgetDto updateBudgetDto)
    {
        try
        {
            var budget = await _mediator.Send(new UpdateBudgetCommand(id, updateBudgetDto));
            return Ok(budget);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}