using GatewayService.Api.Middleware;
using GatewayService.Application.Common;
using GatewayService.Application.Features.Pay;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IWebHostEnvironment env = builder.Environment;

IConfigurationRoot configuration = CreateConfiguration();

IConfigurationSection serviceUrlsSection = configuration.GetSection("ServiceUrls");
builder.Services.Configure<ServiceUrls>(serviceUrlsSection);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(PayCommandHandler).Assembly));
builder.Services.AddHttpClient();

var app = builder.Build();

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