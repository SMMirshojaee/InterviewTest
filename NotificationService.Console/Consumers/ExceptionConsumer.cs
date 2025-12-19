using MassTransit;
using SHARE.Model;

namespace NotificationService.Console.Consumers;

public class ExceptionConsumer : IConsumer<ExceptionMessage>
{
    public Task Consume(ConsumeContext<ExceptionMessage> context)
    {
        WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss}:Exception thrown :{context.Message}");
        return Task.CompletedTask;
    }
}