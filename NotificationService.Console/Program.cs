global using static System.Console;
using MassTransit;
using NotificationService.Console.Consumers;

var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.Host("rabbitmq://localhost", h =>
    {
        h.Username("guest");
        h.Password("guest");
    });

    cfg.ReceiveEndpoint("getToken_queue", e =>
    {
        e.Consumer<GetTokenConsumer>();
    });
    cfg.ReceiveEndpoint("updateStatus_queue", e =>
    {
        e.Consumer<UpdateStatusConsumer>();
    });
    cfg.ReceiveEndpoint("verify_queue", e =>
    {
        e.Consumer<VerifyConsumer>();
    });
    cfg.ReceiveEndpoint("exception_queue", e =>
    {
        e.Consumer<ExceptionConsumer>();
    });
    cfg.ReceiveEndpoint("pay_queue", e =>
    {
        e.Consumer<PayConsumer>();
    });
});

// اجرای MassTransit
await busControl.StartAsync();

Console.WriteLine("Notification Service is ready!");
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

await busControl.StopAsync();
