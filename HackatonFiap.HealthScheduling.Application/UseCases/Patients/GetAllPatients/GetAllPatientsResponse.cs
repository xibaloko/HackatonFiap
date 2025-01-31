namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetAllPatients
{
    public class UpdatePatientResponse
    {
        public IEnumerable<PatientResponse> Patients { get; init; } = [];
    }

    public class PatientResponse
    {
        public Guid Uuid { get; init; }
        public required string Name { get; init; }
        public required string LastName { get; init; }
        public required string Email { get; init; }
        public required string CPF { get; init; }
        public required string RG { get; init; }
    }
}
