using Application.Rfxes.Commands;
using Application.Rfxes.DTOs;
using Application.Rfxes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RfxController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List() =>
        Ok(await mediator.Send(new GetRfxListQuery()));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var data = await mediator.Send(new GetRfxByIdQuery(id));
        return data is null ? NotFound() : Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RfxUpsertDto dto)
    {
        var id = await mediator.Send(new CreateRfxCommand(dto));
        return CreatedAtAction(nameof(Get), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] RfxUpsertDto dto)
    {
        var ok = await mediator.Send(new UpdateRfxCommand(id, dto));
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await mediator.Send(new DeleteRfxCommand(id));
        return ok ? NoContent() : NotFound();
    }
}
