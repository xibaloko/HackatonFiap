using System.Linq.Expressions;

namespace HackatonFiap.HealthScheduling.Domain.Entities.Bases.Repositories;

public interface IBaseRepository<T> where T : EntityBase
{
    T? Find(int id);
    Task<T?> FindAsync(int id, CancellationToken cancellationToken = default);
    T? FirstOrDefault(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool isTracking = true);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool isTracking = true, CancellationToken cancellationToken = default);
    IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null, bool isTracking = true);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null, bool isTracking = true, CancellationToken cancellationToken = default);
    void Add(T entity);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void AddBulk(IEnumerable<T> entityes);
    public void Update(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entity);
}
