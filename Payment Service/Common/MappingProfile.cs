using AutoMapper;
using PaymentService.Api.Models;
using PaymentService.Application.Payments.GetToken;

namespace PaymentService.Api.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TokenRequest, GetTokenCommand>();
        }
    }
}
