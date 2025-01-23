namespace HackatonFiap.HealthScheduling.Domain.Entities.Bases;

public abstract class EntityBase
{
    public int Id { get; private set; }
    public Guid Uuid { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }

    public EntityBase()
    {
        Uuid = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    protected void CreatedNow() => CreatedAt = DateTime.UtcNow;
    public void UpdatedNow() => UpdatedAt = DateTime.UtcNow;
    public void AsSoftDeletable() => IsDeleted = true;
}
