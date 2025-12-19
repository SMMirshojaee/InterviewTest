using MassTransit;
using SHARE.Model;

namespace NotificationService.Console.Consumers;

public class PayConsumer : IConsumer<PayMessage>
{
    public Task Consume(ConsumeContext<PayMessage> context)
    {
        WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss}:Pay :{context.Message}");
        return Task.CompletedTask;
    }
}