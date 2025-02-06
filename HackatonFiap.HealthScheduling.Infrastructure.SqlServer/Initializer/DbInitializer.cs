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

        // 🔹 Evita duplicação de registros
        if (!_db.MedicalSpecialties.Any())
        {
            Console.WriteLine("Populando tabela MedicalSpecialties...");

            _db.MedicalSpecialties.Add(new MedicalSpecialty("Alergologia"));
            _db.MedicalSpecialties.Add(new MedicalSpecialty("Anestesiologia"));
            _db.MedicalSpecialties.Add(new MedicalSpecialty("Angiologia"));
            _db.MedicalSpecialties.Add(new MedicalSpecialty("Cardiologia"));
            _db.MedicalSpecialties.Add(new MedicalSpecialty("Cirurgia Cardiovascular"));
            _db.MedicalSpecialties.Add(new MedicalSpecialty("Cirurgia da Mão"));
            _db.MedicalSpecialties.Add(new MedicalSpecialty("Cirurgia de Cabeça e Pescoço"));
            _db.MedicalSpecialties.Add(new MedicalSpecialty("Cirurgia Geral"));
            _db.MedicalSpecialties.Add(new MedicalSpecialty("Gastroenterologia"));
            _db.MedicalSpecialties.Add(new MedicalSpecialty("Neurologia"));
            _db.MedicalSpecialties.Add(new MedicalSpecialty("Psiquiatria"));
            _db.MedicalSpecialties.Add(new MedicalSpecialty("Urologia"));

            _db.SaveChanges();
            Console.WriteLine("Tabela MedicalSpecialties populada!");
        }
        else
        {
            Console.WriteLine("A tabela MedicalSpecialties já possui dados.");
        }

    }
}

