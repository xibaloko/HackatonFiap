using AutoMapper;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients.Interfaces;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using MediatR;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Persistence;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Persistence.EntitiesRepositories;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.UpdatePatient;

public class UpdatePatientRequestHandler : IRequestHandler<UpdatePatientRequest, Result<UpdatePatientResponse>>
{
    private readonly IRepositories _repositories; // Certifique-se de que a interface está correta
    private readonly IMapper _mapper;

    public UpdatePatientRequestHandler(IRepositories repositories, IMapper mapper)
    {
        _repositories = repositories;
        _mapper = mapper;
    }

 

    public async Task<Result<UpdatePatientResponse>> Handle(UpdatePatientRequest request, CancellationToken cancellationToken)
    {
        // Busca o paciente pelo UUID
        Patient? patient = await _repositories.PatientRepository.FirstOrDefaultAsync(p => p.Uuid == request.Uuid, cancellationToken: cancellationToken);

        if (patient == null)
            return Result.Fail("Paciente não encontrado.");

        // Atualiza os dados do paciente com base na requisição
        Patient? updatedPatient = new Patient
        {
            Name = request.Name,
            LastName = request.LastName,
            Email = request.Email,
            CPF = request.CPF
            //RG = request.RG
        };

        // Atualiza o paciente no repositório
        _repositories.PatientRepository.Update(updatedPatient);

        // Mapeia o paciente atualizado para UpdatePatientResponse
        var response = _mapper.Map<UpdatePatientResponse>(updatedPatient);

        // Retorna o resultado com a resposta mapeada
        return Result.Ok(response);
    }

    //public async Task<UpdatePatientResponse> Handle(UpdatePatientRequest request, CancellationToken cancellationToken)
    //{
    //    Patient? patient = await _repositories.PatientRepository.FirstOrDefaultAsync(patient => patient.Uuid == request.Uuid, cancellationToken: cancellationToken);

    //    if (patient == null)
    //    {
    //        throw new KeyNotFoundException("Paciente não encontrado.");
    //    }

    //    Patient? updatedPatient = new Patient
    //    {
    //        Name = request.Name,
    //        LastName = request.LastName,
    //        Email = request.Email,
    //        CPF = request.CPF
    //       //RG = request.RG
    //    };

    //    _repositories.PatientRepository.Update(updatedPatient);

    //    Patient? patientResponse = await _repositories.PatientRepository.FirstOrDefaultAsync(patient => patient.Uuid == request.Uuid, cancellationToken: cancellationToken);

    //    if (patient == null)
    //    {
    //        throw new KeyNotFoundException("Paciente não encontrado.");
    //    }

    //    return new UpdatePatientResponse
    //    {
    //        Uuid = patientResponse.Uuid,
    //        Name = patientResponse.Name,
    //        LastName = patientResponse.LastName,
    //        Email = patientResponse.Email,
    //        CPF = patientResponse.CPF,
    //        RG = patientResponse.RG
    //    };
    //}
}
