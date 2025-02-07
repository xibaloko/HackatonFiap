using System.Linq.Expressions;
using System.Security.Claims;
using FluentResults;
using HackatonFiap.HealthScheduling.Application.Configurations.FluentResults;
using HackatonFiap.HealthScheduling.Domain.Entities.Doctors;
using HackatonFiap.HealthScheduling.Domain.Entities.Schedules;
using HackatonFiap.HealthScheduling.Domain.PersistenceContracts;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Appointments.GetDoctorAppointments;

public class GetDoctorAppointmentsRequestHandler : IRequestHandler<GetDoctorAppointmentsRequest, Result<GetDoctorAppointmentsResponse>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;

    public GetDoctorAppointmentsRequestHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<GetDoctorAppointmentsResponse>> Handle(GetDoctorAppointmentsRequest request, CancellationToken cancellationToken)
    {
        var identityId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(identityId))
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized: User not found."));

        Doctor? doctor = await _unitOfWork.DoctorRepository.FirstOrDefaultAsync(doctor =>
            doctor.Uuid == request.Uuid && doctor.IsDeleted == false, cancellationToken: cancellationToken);

        if (doctor is null)
            return Result.Fail(ErrorHandler.HandleBadRequest("Doctor not found."));

        if (identityId != doctor.IdentityId!.Value.ToString())
            return Result.Fail(ErrorHandler.HandleUnauthorized("Unauthorized to access the resource."));

        Expression<Func<Schedule, bool>> filter = schedule =>
            schedule.DoctorId == doctor.Id &&
            schedule.Appointments.Count > 0 &&
            schedule.Appointments.Any(x => x.IsDeleted == false) &&
            schedule.Avaliable == false &&
            schedule.IsDeleted == false;

        var schedules = await _unitOfWork.ScheduleRepository.GetAllAsync(
            filter: filter,
            includeProperties: "Appointments,Appointments.Patient",
            isTracking: false,
            cancellationToken: cancellationToken
        );

        var response = GenerateResponse(schedules);

        return Result.Ok(response);
    }

    private static GetDoctorAppointmentsResponse GenerateResponse(IEnumerable<Schedule> schedules)
    {
        var response = new GetDoctorAppointmentsResponse();

        foreach (var schedule in schedules)
        {
            var appointment = schedule.Appointments.Single();

            response.Appointments.Add(new DoctorAppointmentResponse
            {
                Uuid = appointment.Uuid,
                PatientName = appointment.Patient.Name,
                InitialDateHour = schedule.InitialDateHour,
                FinalDateHour = schedule.FinalDateHour,
                MedicalAppointmentPrice = schedule.MedicalAppointmentPrice,
                IsCanceledByPatient = appointment.IsCanceledByPatient,
                CancellationReason = appointment.CancellationReason
            });
        }

        return response;
    }
}