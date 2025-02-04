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
builder.Services.Configure<SendGridOptions>(builder.Configuration.GetSection("SendGridOptions"));

builder.Services.AddSingleton<ISendGridClient>(sp =>
{
    var options = sp.GetRequiredService<IOptions<SendGridOptions>>().Value;
    if (string.IsNullOrEmpty(options.ApiKey))
    {
        throw new Exception("SendGrid API Key não configurada corretamente.");
    }
    return new SendGridClient(options.ApiKey);
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();