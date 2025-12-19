using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Api.Models;
using PaymentService.Application.Models;
using PaymentService.Application.Payments.GetToken;
using PaymentService.Application.Payments.UpdatePaymentStatus;
using PaymentService.Application.Payments.VerifyPayment;

namespace PaymentService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IMediator mediator, IMapper mapper) : ControllerBase
    {
        [HttpPost("get-token")]
        public async Task<IActionResult> GetToken(TokenDto tokenRequest) =>
            Ok(await mediator.Send(mapper.Map<GetTokenCommand>(tokenRequest)));

        [HttpPost("verify")]
        public async Task<IActionResult> Verify(VerifyDto verifyRequest) =>
            Ok(await mediator.Send(mapper.Map<VerifyCommand>(verifyRequest)));

        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateStatus(UpdateStatusDto updateStatusRequest) =>
            Ok(await mediator.Send(mapper.Map<UpdateStatusCommand>(updateStatusRequest)));
    }
}
