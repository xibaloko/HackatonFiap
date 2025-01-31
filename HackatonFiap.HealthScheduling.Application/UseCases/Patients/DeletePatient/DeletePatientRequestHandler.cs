using AutoMapper;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients.Interfaces;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using MediatR;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Persistence;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Persistence.EntitiesRepositories;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.DeletePatient;

public class DeletePatientRequestHandler : IRequestHandler<DeletePatientRequest, Result<DeletePatientResponse>>
{
    private readonly IRepositories _repositories; 
    private readonly IMapper _mapper;

    public DeletePatientRequestHandler(IRepositories repositories, IMapper mapper)
    {
        _repositories = repositories;
        _mapper = mapper;
    }

    public async Task<Result<DeletePatientResponse>> Handle(DeletePatientRequest request, CancellationToken cancellationToken)
    {
        Patient? patient = await _repositories.PatientRepository.FirstOrDefaultAsync(patient => patient.Uuid == request.Uuid, cancellationToken: cancellationToken);

        if (patient == null)
        {
            return new DeletePatientResponse { Success = false, Message = "Paciente não encontrado." };
        }

        _repositories.PatientRepository.Remove(patient);

        return new DeletePatientResponse { Success = true, Message = "Paciente removido com sucesso." };
    }
}
