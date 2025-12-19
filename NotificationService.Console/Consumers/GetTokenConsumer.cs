using MassTransit;
using SHARE.Model;

namespace NotificationService.Console.Consumers;

internal class GetTokenConsumer : IConsumer<GetTokenMessage>
{
    public Task Consume(ConsumeContext<GetTokenMessage> context)
    {
        WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss}:new token generated:{context.Message}");
        return Task.CompletedTask;
    }
}