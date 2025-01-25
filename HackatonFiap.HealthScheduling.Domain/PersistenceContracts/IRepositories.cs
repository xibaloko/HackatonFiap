﻿using HackatonFiap.HealthScheduling.Domain.Entities.Doctors.Interfaces;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients.Interfaces;

namespace HackatonFiap.HealthScheduling.Domain.PersistenceContracts;

public interface IRepositories : IDisposable
{
    IDoctorRepository DoctorRepository { get; }
    IPatientRepository PatientRepository { get; }
    Task SaveAsync(CancellationToken cancellationToken = default);
    void Save();
}
