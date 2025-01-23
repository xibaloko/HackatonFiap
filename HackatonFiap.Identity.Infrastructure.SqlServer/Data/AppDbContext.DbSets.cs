using Microsoft.EntityFrameworkCore;
using HackatonFiap.Identity.Domain.Entities;

namespace HackatonFiap.Identity.Infrastructure.SqlServer.Data;

public sealed partial class AppDbContext
{
    public DbSet<ApplicationUser> UserAccounts { get; private set; }
}
