using PaymentService.Api.Common;
using PaymentService.Application.Common;
using PaymentService.Application.Payments.GetToken;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PaymentService.Infrastructure.Persistence;
using Microsoft.Extensions.Options;
using PaymentService.Api.Middleware;
using Hangfire;
using PaymentService.Infrastructure.BackgroundJobs;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IWebHostEnvironment env = builder.Environment;

IConfigurationRoot configuration = CreateConfiguration();

IConfigurationSection serviceUrlsSection = configuration.GetSection("ServiceUrls");
builder.Services.Configure<ServiceUrls>(serviceUrlsSection);

IConfigurationSection appSettingsSection = configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
AppSettings appSetting = appSettingsSection.Get<AppSettings>()!;

builder.Services.AddControllers();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetTokenCommandHandler).Assembly));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>((context, containerBuilder) =>
{
    // اضافه کردن Module خود به کانتینر
    containerBuilder.RegisterModule(new AutofacDi());
});
builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseSqlServer(appSetting.ConnectionString));

builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(appSetting.ConnectionString));
builder.Services.AddHangfireServer();
builder.Services.AddTransient<TransactionsManagement>();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

var app = builder.Build();
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard();

var jobClass = app.Services.GetRequiredService<TransactionsManagement>();

app.Services.GetRequiredService<IRecurringJobManager>()
    .AddOrUpdate(
        "ExpirationCheckJob",
        () => jobClass.ExpirationCheckJob(),
        "*/30 * * * * *"
    );
app.Run();
IConfigurationRoot CreateConfiguration()
{
    IConfigurationBuilder configBuilder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();
    return configBuilder.Build();
}