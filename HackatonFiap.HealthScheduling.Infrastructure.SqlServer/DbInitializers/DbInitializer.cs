using HackatonFiap.HealthScheduling.Domain.Entities;
using HackatonFiap.HealthScheduling.Domain.Entities.MedicalSpecialties;
using HackatonFiap.HealthScheduling.Domain.Entities.Bases.Repositories;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.DbInitializers;


public class DbInitializer : IDbInitializer
{
    private readonly IServiceProvider _serviceProvider;

    public DbInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Initialize()
    {
        using (var context = new AppDbContext(_serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>() ))
        {
            var migrations = context.Database.GetPendingMigrations();
            if (migrations.Any())
            {
                Console.WriteLine("Aplicando migrations pendentes...");
                context.Database.Migrate();
                Console.WriteLine("Migrations aplicadas com sucesso!");
            }
            else
            {
                Console.WriteLine("Nenhuma migration pendente.");
            }
            
            if (!context.MedicalSpecialties.Any())
            {
                Console.WriteLine("Populando tabela MedicalSpecialties...");

                context.MedicalSpecialties.Add(new MedicalSpecialty("Alergologia"));
                context.MedicalSpecialties.Add(new MedicalSpecialty("Anestesiologia"));
                context.MedicalSpecialties.Add(new MedicalSpecialty("Angiologia"));
                context.MedicalSpecialties.Add(new MedicalSpecialty("Cardiologia"));
                context.MedicalSpecialties.Add(new MedicalSpecialty("Cirurgia Cardiovascular"));
                context.MedicalSpecialties.Add(new MedicalSpecialty("Cirurgia da Mão"));
                context.MedicalSpecialties.Add(new MedicalSpecialty("Cirurgia de Cabeça e Pescoço"));
                context.MedicalSpecialties.Add(new MedicalSpecialty("Cirurgia Geral"));
                context.MedicalSpecialties.Add(new MedicalSpecialty("Gastroenterologia"));
                context.MedicalSpecialties.Add(new MedicalSpecialty("Neurologia"));
                context.MedicalSpecialties.Add(new MedicalSpecialty("Psiquiatria"));
                context.MedicalSpecialties.Add(new MedicalSpecialty("Urologia"));

                context.SaveChanges();
                Console.WriteLine("Tabela MedicalSpecialties populada!");
            }
            else
            {
                Console.WriteLine("A tabela MedicalSpecialties já possui dados.");
            }
        }
    }
}

