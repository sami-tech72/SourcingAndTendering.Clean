using Application.Rfxes.Commands.CreateRfx;
using Application.Rfxes.Commands.DeleteRfx;
using Application.Rfxes.Commands.UpdateRfx;
using Application.Rfxes.DTOs;
using Application.Rfxes.Queries.GetRfxById;
using Application.Rfxes.Queries.GetRfxList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RfxController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<RfxListItemVm>>> GetAll()
        => await mediator.Send(new GetRfxListQuery());

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RfxUpsertDto>> GetById(Guid id)
        => await mediator.Send(new GetRfxByIdQuery(id));

    //[HttpPost]
    //public async Task<ActionResult<object>> Create([FromBody] RfxUpsertDto dto)
    //{
    //    var res = await mediator.Send(new CreateRfxCommand(dto));
    //    return Ok(new { id = res.Id, code = res.Code });
    //}

    [HttpPost]
    public async Task<ActionResult<object>> Create([FromBody] RfxUpsertDto dto)
    {
        var res = await mediator.Send(new CreateRfxCommand(dto));
        // return token if published; UI can build the URL as `${origin}/tenders/${token}`
        return Ok(new { id = res.Id, code = res.Code, publicToken = res.PublicToken });
    }


    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] RfxUpsertDto dto)
    {
        await mediator.Send(new UpdateRfxCommand(id, dto));
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await mediator.Send(new DeleteRfxCommand(id));
        return NoContent();
    }
}
