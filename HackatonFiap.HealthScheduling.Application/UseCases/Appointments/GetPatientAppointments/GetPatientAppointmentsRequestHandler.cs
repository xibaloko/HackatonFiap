
using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Appointments;
using HackatonFiap.HealthScheduling.Domain.Entities.Patients;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Appointments.GetPatientAppointments;

public class GetPatientAppointmentsRequestHandler : IRequestHandler<GetPatientAppointmentsRequest, Result<GetPatientAppointmentsResponse>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPatientAppointmentsRequestHandler(IUnitOfWork repositories, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = repositories;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<GetPatientAppointmentsResponse>> Handle(GetPatientAppointmentsRequest request, CancellationToken cancellationToken)
    {
        var identityId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        Patient? patient = await _unitOfWork.PatientRepository.FirstOrDefaultAsync(patient =>
            patient.Uuid == request.Uuid && patient.IsDeleted == false, cancellationToken: cancellationToken);

        if (string.IsNullOrWhiteSpace(identityId))
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized: User not found"));

        Expression<Func<Appointment, bool>> filter = appointment =>
            appointment.Patient.Uuid == request.Uuid &&
            !appointment.IsDeleted;

        var appointments = await _unitOfWork.AppointmentRepository.GetAllAsync(
            filter: filter,
            includeProperties: "Patient,Schedule,Schedule.Doctor",
            isTracking: false,
            cancellationToken: cancellationToken
        );

        if (patient is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("Patient not found."));

        if (identityId != patient.IdentityId!.Value.ToString())
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized to access the resource."));

        var response = _mapper.Map<GetPatientAppointmentsResponse>(appointments);

        return Result.Ok(response);
    }

}