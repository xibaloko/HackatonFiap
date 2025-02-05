namespace HackatonFiap.HealthScheduling.Application.UseCases.MedicalSpecialties.GetAllMedicalSpecialties;

public class GetAllMedicalSpecialtiesResponse
{
    public IEnumerable<MedicalSpecialtyResponse> MedicalSpecialties { get; init; } = [];
}

public class MedicalSpecialtyResponse
{
    public Guid Uuid { get; init; }
    public required string Description { get; init; }

}
