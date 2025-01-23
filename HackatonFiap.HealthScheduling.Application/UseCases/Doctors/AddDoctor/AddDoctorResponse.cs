namespace HackatonFiap.HealthScheduling.Application.UseCases.Doctors.AddDoctor;

public sealed class AddDoctorResponse
{
    public int Id { get; init; }
    public Guid Uuid { get; init; }
    public required string Name { get; init; }
    public required string LastName { get; init; }
}
