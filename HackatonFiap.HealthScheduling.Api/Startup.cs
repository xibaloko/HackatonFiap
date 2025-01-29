﻿using Asp.Versioning;
using Asp.Versioning.Conventions;
using HackatonFiap.HealthScheduling.Application;
using HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetPatient;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients.Interfaces;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Persistence.EntitiesRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HackatonFiap.HealthScheduling.Infrastructure.RabbitMq.Configurations;

namespace HackatonFiap.HealthScheduling.Api;

public class Startup
{
    private const int ApiDefaultMajorVersion = 1;
    private const string ApiVersionHeader = "x-api-version";
    private const string ApiVersionGroupNameFormat = "'v'V";

    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {

        // Registra o repositório no DI
        services.AddScoped<IPatientRepository, PatientRepository>();

        // Registra o MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetPatientRequestHandler).Assembly));


        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        //TODO: Adicionar ao IoC
        services.Configure<RabbitMqSettings>(_configuration.GetSection("RabbitMQ"));

        services.AddApplicationModule(_configuration);

        services.AddHttpClient();

        services.AddApiVersioning(configuration =>
        {
            configuration.DefaultApiVersion = new ApiVersion(ApiDefaultMajorVersion);
            configuration.AssumeDefaultVersionWhenUnspecified = true;
            configuration.ReportApiVersions = true;
            configuration.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(), new HeaderApiVersionReader(ApiVersionHeader), new MediaTypeApiVersionReader(ApiVersionHeader));
        }).AddMvc(configuration =>
        {
            configuration.Conventions.Add(new VersionByNamespaceConvention());
        }).AddApiExplorer(configuration =>
        {
            configuration.GroupNameFormat = ApiVersionGroupNameFormat;
            configuration.SubstituteApiVersionInUrl = true;
        });
    }

    public void Configure(WebApplication app)
    {
        if (_environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapControllers();
    }
}
