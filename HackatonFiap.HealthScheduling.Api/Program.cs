using HackatonFiap.HealthScheduling.Api;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients.Interfaces;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Persistence.EntitiesRepositories;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration, builder.Environment);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app);

app.Run();