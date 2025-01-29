using HackatonFiap.EmailProvider.Worker;
using HackatonFiap.EmailProvider.Worker.Configurations;

var builder = Host.CreateApplicationBuilder(args);

var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.Configure<SendGridOptions>(builder.Configuration.GetSection("SendGridOptions"));
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));


builder.Services.AddHostedService<Worker>();
builder.Services.Configure<SendGridOptions>(options =>
{
    options.ApiKey = "chavedosite";
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
