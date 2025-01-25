using HackatonFiap.EmailProvider.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();
builder.Services.Configure<SendGridOptions>(options =>
{
    options.ApiKey = "chavedosite";
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
