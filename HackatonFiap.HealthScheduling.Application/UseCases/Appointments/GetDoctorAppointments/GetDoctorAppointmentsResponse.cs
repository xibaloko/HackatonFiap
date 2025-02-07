namespace HackatonFiap.HealthScheduling.Application.UseCases.Appointments.GetDoctorAppointments;

public class GetDoctorAppointmentsResponse()
{
    public List<DoctorAppointmentResponse> Appointments { get; init; } = [];
}

public class DoctorAppointmentResponse
{
    public Guid Uuid { get; init; }
    public required DateTime InitialDateHour { get; init; }
    public required DateTime FinalDateHour { get; init; }
    public required decimal MedicalAppointmentPrice { get; init; }
    public required string PatientName { get; init; }
    public required bool IsCanceledByPatient { get; init; }
    public string? CancellationReason { get; init; }
}
