using HackatonFiap.HealthScheduling.Domain.Entities;
using HackatonFiap.HealthScheduling.Domain.Entities.Bases.Repositories;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.DbInitializer.DbInitializer;


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

            if (context.MedicalSpecialties.Any())
            {
                return;
            }

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Alergologia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Anestesiologia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Angiologia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Cardiologia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Cirurgia Cardiovascular",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Cirurgia da Mão",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Cirurgia de Cabeça e Pescoço",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Cirurgia do Aparelho Digestivo",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Cirurgia Geral",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Cirurgia Oncológica",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Cirurgia Pediátrica",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Cirurgia Plástica",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Cirurgia Torácica",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Cirurgia Vascular",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Clínica Médica",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Coloproctologia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Dermatologia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Endocrinologia e Metabologia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Endoscopia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Gastroenterologia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Genética Médica",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Geriatria",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Ginecologia e Obstetrícia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Hematologia e Hemoterapia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Homeopatia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Infectologia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Mastologia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Medicina de Emergência",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Medicina de Família e Comunidade",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Medicina do Trabalho",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Medicina Esportiva",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Medicina Física e Reabilitação",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Medicina Intensiva",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Medicina Legal e Perícia Médica",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Medicina Nuclear",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Medicina Preventiva e Social",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Nefrologia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Neurocirurgia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Neurologia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Nutrologia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Oftalmologia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Oncologia Clínica",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Ortopedia e Traumatologia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Otorrinolaringologia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Patologia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Patologia Clínica/Medicina Laboratorial",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Pediatria",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Pneumologia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Psiquiatria",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Radiologia e Diagnóstico por Imagem",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Radioterapia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Reumatologia",
            });

            context.MedicalSpecialties.Add(new MedicalSpecialty
            {
                Description: "Urologia",
            });

            context.SaveChanges();
        }
    }
}

