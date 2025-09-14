using Application.Rfxes.DTOs;
using Application.Rfxes.Queries.GetPublicRfxByToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/public/rfx")]
public class PublicRfxController(IMediator mediator) : ControllerBase
{
    [HttpGet("{token}")]
    public async Task<ActionResult<PublicRfxDto>> GetByToken(string token)
        => await mediator.Send(new GetPublicRfxByTokenQuery(token));
}
