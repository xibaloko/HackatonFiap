using HackatonFiap.HealthScheduling.Domain.Entities;
using HackatonFiap.HealthScheduling.Domain.Entities.MedicalSpecialties;
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

            context.MedicalSpecialtys.Add(new MedicalSpecialty
(
    Description: "Alergologia",

));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Anestesiologia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Angiologia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Cardiologia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Cirurgia Cardiovascular",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Cirurgia da Mão",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Cirurgia de Cabeça e Pescoço",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Cirurgia do Aparelho Digestivo",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Cirurgia Geral",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Cirurgia Oncológica",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Cirurgia Pediátrica",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Cirurgia Plástica",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Cirurgia Torácica",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Cirurgia Vascular",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Clínica Médica",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Coloproctologia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Dermatologia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Endocrinologia e Metabologia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Endoscopia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Gastroenterologia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Genética Médica",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Geriatria",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Ginecologia e Obstetrícia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Hematologia e Hemoterapia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Homeopatia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Infectologia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Mastologia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Medicina de Emergência",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Medicina de Família e Comunidade",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Medicina do Trabalho",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Medicina Esportiva",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Medicina Física e Reabilitação",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Medicina Intensiva",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Medicina Legal e Perícia Médica",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Medicina Nuclear",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Medicina Preventiva e Social",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Nefrologia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Neurocirurgia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Neurologia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Nutrologia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Oftalmologia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Oncologia Clínica",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Ortopedia e Traumatologia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Otorrinolaringologia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Patologia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Patologia Clínica/Medicina Laboratorial",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Pediatria",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Pneumologia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Psiquiatria",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Radiologia e Diagnóstico por Imagem",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Radioterapia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Reumatologia",
            ));

            context.MedicalSpecialtys.Add(new MedicalSpecialty
            (
                Description: "Urologia",
            ));

            context.SaveChanges();
        }
    }
}

