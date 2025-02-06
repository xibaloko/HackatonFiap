using HackatonFiap.HealthScheduling.Domain.Entities.Appointments.Repositories;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors.Interfaces;
using HackatonFiap.HealthScheduling.Domain.Entities.MedicalSpecialties.Interfaces;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients.Interfaces;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules.Interfaces;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Data;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Persistence.EntitiesRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    public IDbContextTransaction _transaction = default!;

    public IDoctorRepository DoctorRepository { get; private set; }
    public IPatientRepository PatientRepository { get; private set; }
    public IScheduleRepository ScheduleRepository { get; private set; }
    public IAppointmentRepository AppointmentRepository { get; private set; }
    public IMedicalSpecialtyRepository MedicalSpecialtyRepository { get; private set; }

    public UnitOfWork(AppDbContext db)
    {
        _db = db;
        DoctorRepository = new DoctorRepository(_db);
        PatientRepository = new PatientRepository(_db);
        ScheduleRepository = new ScheduleRepository(_db);
        AppointmentRepository = new AppointmentRepository(_db);
        MedicalSpecialtyRepository = new MedicalSpecialtyRepository(_db);
    }

    public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) 
        => _transaction = _db.Database.BeginTransaction(isolationLevel);

    public async Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default)
        => _transaction = await _db.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
    public void Commit() 
        => _transaction.Commit();
    public async Task CommitAsync(CancellationToken cancellationToken = default) 
        => await _transaction.CommitAsync(cancellationToken);

    public void Rollback() 
        => _transaction.Rollback();

    public Task RollbackAsync(CancellationToken cancellationToken = default) 
        => _transaction.RollbackAsync(cancellationToken);

    public async Task SaveAsync(CancellationToken cancellationToken = default)
        => await _db.SaveChangesAsync(cancellationToken);

    public void Dispose() 
        => _db.Dispose();
}