using HackatonFiap.Identity.Domain.Entities;
using HackatonFiap.Identity.Domain.Services;
using HackatonFiap.Identity.Infrastructure.SqlServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HackatonFiap.Identity.Infrastructure.SqlServer.Initializer;

public sealed class DbInitializer : IDbInitializer
{
    private readonly AppDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DbInitializer(AppDbContext db,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _db = db;
        _roleManager = roleManager;
        _userManager = userManager;
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
        
        if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
        {
            Console.WriteLine("Criando roles...");
            _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole("Doctor")).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole("Patient")).GetAwaiter().GetResult();
            Console.WriteLine("Roles criadas com sucesso!");
        }
        
        ApplicationUser? user = _db.UserAccounts.FirstOrDefaultAsync(u => u.Email == "admin@admin.com").GetAwaiter().GetResult();

        if (user is null)
        {
            Console.WriteLine("Criando usuário Admin...");
            var result = _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@admin.com",
                EmailConfirmed = true,
            }, "Admin01++").GetAwaiter().GetResult();

            if (result.Succeeded)
            {
                user = _db.UserAccounts.Single(u => u.Email == "admin@admin.com");
                _userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
                Console.WriteLine("Usuário Admin criado com sucesso!");
            }
        }
        else
        {
            Console.WriteLine("Usuário Admin já existe.");
        }
    }

}

