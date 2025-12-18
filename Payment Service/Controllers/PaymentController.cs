using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Api.Models;
using PaymentService.Application.Models;
using PaymentService.Application.Payments.GetToken;

namespace PaymentService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PaymentController(IMediator mediator, IMapper mapper)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpPost("get-token")]
        public async Task<IActionResult> GetToken(TokenRequest tokenRequest)
        {
            TokenResponse response = await _mediator.Send(_mapper.Map<GetTokenCommand>(tokenRequest));
            return Ok(response);
        }
    }
}
