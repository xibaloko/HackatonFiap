namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.UpdatePatient
{
    public class UpdatePatientResponse
    {
        public Guid Uuid { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string CPF { get; set; }
        public required string RG { get; set; }
        public bool Success { get; set; }
    }
}
