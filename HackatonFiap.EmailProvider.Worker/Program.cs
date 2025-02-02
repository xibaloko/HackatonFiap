using HackatonFiap.EmailProvider.Worker;
using HackatonFiap.EmailProvider.Worker.Configurations;
using Microsoft.Extensions.Options;
using SendGrid;

var builder = Host.CreateApplicationBuilder(args);

var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton<ISendGridClient>(sp =>
{
    var options = sp.GetRequiredService<IOptions<SendGridOptions>>().Value;
    return new SendGridClient(options.ApiKey);
});

builder.Services.AddHostedService<Worker>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
