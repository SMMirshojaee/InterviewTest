using GatewayService.Application.Features.Pay;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GatewayService.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GatewayController(IMediator mediator) : ControllerBase
    {
        [HttpGet("{token}")]
        public async Task<IActionResult> Pay([FromRoute] Guid token) =>
            Ok(await mediator.Send(new PayCommand(token)));
    }
}
