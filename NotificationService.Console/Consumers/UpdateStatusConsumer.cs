using MassTransit;
using SHARE.Model;

namespace NotificationService.Console.Consumers;

internal class UpdateStatusConsumer : IConsumer<UpdateStatusMessage>
{
    public Task Consume(ConsumeContext<UpdateStatusMessage> context)
    {
        WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss}:Update Transaction :{context.Message}");
        return Task.CompletedTask;
    }
}