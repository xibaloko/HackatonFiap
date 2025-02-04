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
        using (var context = new AppDbContext(_serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
        {
            context.Database.EnsureCreated();
            
            var migrations = context.Database.GetPendingMigrations();

            if (migrations.Any())
               context.Database.Migrate();

            if (context.MedicalSpecialties.Any())
            {
                return;
            }

            context.MedicalSpecialties.Add(new MedicalSpecialty("Alergologia"));

            context.MedicalSpecialties.Add(new MedicalSpecialty("Anestesiologia"));

            context.MedicalSpecialties.Add(new MedicalSpecialty("Angiologia"));

            context.MedicalSpecialties.Add(new MedicalSpecialty("Cardiologia"));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Cirurgia Cardiovascular"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Cirurgia da Mão"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Cirurgia de Cabeça e Pescoço"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Cirurgia do Aparelho Digestivo"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Cirurgia Geral"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Cirurgia Oncológica"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Cirurgia Pediátrica"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Cirurgia Plástica"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Cirurgia Torácica"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Cirurgia Vascular"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Clínica Médica"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Coloproctologia"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Dermatologia"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Endocrinologia e Metabologia"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Endoscopia"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Gastroenterologia"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Genética Médica"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Geriatria"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Ginecologia e Obstetrícia"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Hematologia e Hemoterapia"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Homeopatia"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Infectologia"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Mastologia"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Medicina de Emergência"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Medicina de Família e Comunidade"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Medicina do Trabalho"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Medicina Esportiva"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Medicina Física e Reabilitação"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Medicina Intensiva"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Medicina Legal e Perícia Médica"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Medicina Nuclear"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Medicina Preventiva e Social"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Nefrologia"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Neurocirurgia"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Neurologia"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Nutrologia"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Oftalmologia"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Oncologia Clínica"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Ortopedia e Traumatologia"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Otorrinolaringologia"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Patologia"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Patologia Clínica/Medicina Laboratorial"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Pediatria"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Pneumologia"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Psiquiatria"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Radiologia e Diagnóstico por Imagem"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Radioterapia"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Reumatologia"
            ));

            context.MedicalSpecialties.Add(new MedicalSpecialty
            (
               "Urologia"
            ));

            context.SaveChanges();
        }
    }
}

