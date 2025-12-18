using AutoMapper;
using PaymentService.Api.Models;
using PaymentService.Application.Payments.GetToken;
using PaymentService.Application.Payments.UpdatePaymentStatus;
using PaymentService.Application.Payments.VerifyPayment;

namespace PaymentService.Api.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TokenDto, GetTokenCommand>();
            CreateMap<VerifyDto, VerifyCommand>();
            CreateMap<UpdateStatusDto, UpdateStatusCommand>();
        }
    }
}
