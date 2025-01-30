namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.GetDoctorByUuid;

public sealed class GetDoctorByUuidResponse
{
    public Guid Uuid { get; init; }
    public required string Name { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string CPF { get; init; }
    public required string CRM { get; init; }
}
