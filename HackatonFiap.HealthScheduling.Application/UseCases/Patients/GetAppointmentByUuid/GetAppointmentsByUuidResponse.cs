using FluentResults;
using MediatR;

namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAppointmentByUuid;

public class GetAppointmentsByUuidResponse()
{
    public IEnumerable<AppointmentResponse> Appointments { get; init; } = [];
}

public class AppointmentResponse
{
    public Guid Uuid { get; init; }
    public required DateTime InitialDateHour { get; init; }
    public required DateTime FinalDateHour { get; init; }
    public required decimal MedicalAppointmentPrice { get; init; }
    public required string DoctorName { get; init; }
    public required bool IsCanceledByPatient { get; init; }
    public string? CancellationReason { get; init; }
}
