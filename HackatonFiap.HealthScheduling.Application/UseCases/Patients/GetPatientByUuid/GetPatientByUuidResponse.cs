namespace HackatonFiap.HealthScheduling.Application.UseCases.Patients.GetPatientByUuid;

public class GetPatientByUuidResponse
{
    public Guid Uuid { get; init; }
    public required string Name { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string CPF { get; init; }
    public required string RG { get; init; }
}
