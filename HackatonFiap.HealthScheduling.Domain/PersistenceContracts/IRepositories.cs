using HackatonFiap.HealthScheduling.Domain.Entities.Appointments.Repositories;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors.Interfaces;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients.Interfaces;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules.Interfaces;

namespace HackatonFiap.HealthScheduling.Domain.PersistenceContracts;

public interface IRepositories : IDisposable
{
    IDoctorRepository DoctorRepository { get; }
    IPatientRepository PatientRepository { get; }
    IScheduleRepository ScheduleRepository { get; }
    IAppointmentRepository AppointmentRepository { get; }
    Task SaveAsync(CancellationToken cancellationToken = default);
    void Save();
}
