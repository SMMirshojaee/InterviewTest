using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using GatewayService.Application.Common;
using GatewayService.Application.Common.Enums;
using GatewayService.Application.Models;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SHARE.Model;

namespace GatewayService.Application.Features.Pay;

public class PayCommandHandler(HttpClient client, IOptions<ServiceUrls> serviceUrls, IPublishEndpoint publisher)
    : IRequestHandler<PayCommand, PayResponse>
{
    public async Task<PayResponse> Handle(PayCommand request, CancellationToken cancellationToken)
    {
        HttpResponseMessage? response = await client.PostAsJsonAsync(serviceUrls.Value.PaymentServiceVerifyUrl, new VerifyDto
        {
            Token = request.Token,
            AppCode = CreateRandomString(8)
        }, cancellationToken: cancellationToken);
        if (response is null or { IsSuccessStatusCode: false })
        {
            return new PayResponse(false, request.Token, null, null, null, null);
        }
        string verifyResponseString = await response.Content.ReadAsStringAsync(cancellationToken);
        VerifyResponse? verifyResponse = JsonConvert.DeserializeObject<VerifyResponse>(verifyResponseString);
        if (verifyResponse is null)
        {
            return new PayResponse(false, request.Token, null, null, null, null);
        }
        if (verifyResponse.Status == nameof(PaymentStatus.Pending))
        {
            bool isSuccessful = new Random().NextDouble() <= 0.8;
            string? rrn = isSuccessful ? CreateRandomString(12) : null;

            await publisher.Publish(new PayMessage(request.Token, isSuccessful, rrn), cancellationToken);

            StringContent updateContent = new StringContent(JsonConvert.SerializeObject(new UpdateStatusDto
            {
                IsSuccess = isSuccessful,
                Rrn = rrn,
                Token = request.Token
            }), Encoding.UTF8, "application/json");

            await client.PostAsync($"{serviceUrls.Value.PaymentServiceUpdateStatusUrl}", updateContent, cancellationToken);

            return new PayResponse(isSuccessful, request.Token, rrn, verifyResponse.Amount, isSuccessful ? "پرداخت با موفقیت انجام شد" : "پرداخت ناموفق بود", verifyResponse.RedirectUrl);

        }

        return new PayResponse(verifyResponse.IsSuccess, request.Token, verifyResponse.Rrn, verifyResponse.Amount, verifyResponse.Message, verifyResponse.RedirectUrl);

        string CreateRandomString(byte length)
        {
            char[] _chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

            byte[] data = new byte[length];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(data);
            StringBuilder result = new StringBuilder(length);
            foreach (byte b in data)
            {
                result.Append(_chars[b % _chars.Length]);
            }

            return result.ToString();
        }
    }
}