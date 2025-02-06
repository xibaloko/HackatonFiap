using HackatonFiap.HealthScheduling.Domain.Entities.MedicalSpecialties;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Data;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Initializer;


public sealed class DbInitializer : IDbInitializer
{
    private readonly AppDbContext _db;

    public DbInitializer(AppDbContext db)
    {
        _db = db;
    }

    public void Initialize()
    {
        var migrations = _db.Database.GetPendingMigrations();
        if (migrations.Any())
        {
            Console.WriteLine("Aplicando migrations pendentes...");
            _db.Database.Migrate();
            Console.WriteLine("Migrations aplicadas com sucesso!");
        }
        else
        {
            Console.WriteLine("Nenhuma migration pendente.");
        }

        if (!_db.MedicalSpecialties.Any())
        {
            Console.WriteLine("Populando tabela MedicalSpecialties...");

            var specialties = new List<MedicalSpecialty>
            {
                new("Alergologia"),
                new("Anestesiologia"),
                new("Angiologia"),
                new("Cardiologia"),
                new("Cirurgia Cardiovascular"),
                new("Cirurgia da Mão"),
                new("Cirurgia de Cabeça e Pescoço"),
                new("Cirurgia do Aparelho Digestivo"),
                new("Cirurgia Geral"),
                new("Cirurgia Oncológica"),
                new("Cirurgia Pediátrica"),
                new("Cirurgia Plástica"),
                new("Cirurgia Torácica"),
                new("Cirurgia Vascular"),
                new("Clínica Médica"),
                new("Coloproctologia"),
                new("Dermatologia"),
                new("Endocrinologia e Metabologia"),
                new("Endoscopia"),
                new("Gastroenterologia"),
                new("Genética Médica"),
                new("Geriatria"),
                new("Ginecologia e Obstetrícia"),
                new("Hematologia e Hemoterapia"),
                new("Homeopatia"),
                new("Infectologia"),
                new("Mastologia"),
                new("Medicina de Emergência"),
                new("Medicina de Família e Comunidade"),
                new("Medicina do Trabalho"),
                new("Medicina Esportiva"),
                new("Medicina Física e Reabilitação"),
                new("Medicina Intensiva"),
                new("Medicina Legal e Perícia Médica"),
                new("Medicina Nuclear"),
                new("Medicina Preventiva e Social"),
                new("Nefrologia"),
                new("Neurocirurgia"),
                new("Neurologia"),
                new("Nutrologia"),
                new("Oftalmologia"),
                new("Oncologia Clínica"),
                new("Ortopedia e Traumatologia"),
                new("Otorrinolaringologia"),
                new("Patologia"),
                new("Patologia Clínica/Medicina Laboratorial"),
                new("Pediatria"),
                new("Pneumologia"),
                new("Psiquiatria"),
                new("Radiologia e Diagnóstico por Imagem"),
                new("Radioterapia"),
                new("Reumatologia"),
                new("Urologia")
            };

            _db.MedicalSpecialties.AddRange(specialties);
            _db.SaveChanges();
            Console.WriteLine("Tabela MedicalSpecialties populada!");
        }
        else
        {
            Console.WriteLine("A tabela MedicalSpecialties já possui dados.");
        }

    }
}

