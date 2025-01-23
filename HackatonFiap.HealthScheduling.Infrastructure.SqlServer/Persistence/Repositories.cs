﻿using HackatonFiap.HealthScheduling.Domain.Entities.Doctors.Repositories;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Data;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Persistence.EntitiesRepositories;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Persistence;

public class Repositories : IRepositories
{
    private readonly AppDbContext _db;
    public IDoctorRepository DoctorRepository { get; private set; }
    public Repositories(AppDbContext db)
    {
        _db = db;
        DoctorRepository = new DoctorRepository(_db);
    }

    public void Save()
    {
        _db.SaveChanges();
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}