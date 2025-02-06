using HackatonFiap.HealthScheduling.Domain.Entities.Appointments.Repositories;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors.Interfaces;
using HackatonFiap.HealthScheduling.Domain.Entities.MedicalSpecialties.Interfaces;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients.Interfaces;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules.Interfaces;
using System.Data;

namespace HackatonFiap.HealthScheduling.Domain.PersistenceContracts;

public interface IUnitOfWork : IDisposable
{
    IDoctorRepository DoctorRepository { get; }
    IPatientRepository PatientRepository { get; }
    IScheduleRepository ScheduleRepository { get; }
    IAppointmentRepository AppointmentRepository { get; }
    IMedicalSpecialtyRepository MedicalSpecialtyRepository { get; }

    void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);
    void Commit();
    Task CommitAsync(CancellationToken cancellationToken = default);
    void Rollback();
    Task RollbackAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(CancellationToken cancellationToken = default);
}
