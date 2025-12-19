using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using SHARE.Model;

namespace NotificationService.Console.Consumers
{
    public class VerifyConsumer : IConsumer<VerifyMessage>
    {
        public Task Consume(ConsumeContext<VerifyMessage> context)
        {
            WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss}:Try to verify :{context.Message}");
            return Task.CompletedTask;
        }
    }
}
