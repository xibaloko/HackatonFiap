﻿using AutoMapper;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using MediatR;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;


namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetPatientByUuid;

public sealed class GetPatientByUuidRequestHandler : IRequestHandler<GetPatientByUuidRequest, Result<GetPatientByUuidResponse>> 
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPatientByUuidRequestHandler(IUnitOfWork repositories, IMapper mapper)
    {
        _unitOfWork = repositories;
        _mapper = mapper;
    }

    public async Task<Result<GetPatientByUuidResponse>> Handle(GetPatientByUuidRequest request, CancellationToken cancellationToken)
    {
        Patient? patient = await _unitOfWork.PatientRepository.FirstOrDefaultAsync(patient =>
            patient.Uuid == request.Uuid && patient.IsDeleted == false, cancellationToken: cancellationToken);

        if (patient is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("Patient not found."));

        GetPatientByUuidResponse response = _mapper.Map<GetPatientByUuidResponse>(patient);

        return Result.Ok(response);
    }

}
