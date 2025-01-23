using HackatonFiap.HealthScheduling.Domain.Entities.Doctors.Repositories;

namespace HackatonFiap.HealthScheduling.Domain.PersistenceContracts;

public interface IRepositories : IDisposable
{
    IDoctorRepository DoctorRepository { get; }
    Task SaveAsync(CancellationToken cancellationToken = default);
    void Save();
}
