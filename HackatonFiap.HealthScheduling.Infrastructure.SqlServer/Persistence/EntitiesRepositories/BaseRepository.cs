using HackatonFiap.HealthScheduling.Domain.Entities.Bases;
using HackatonFiap.HealthScheduling.Domain.Entities.Bases.Repositories;
using HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HackatonFiap.HealthScheduling.Infrastructure.SqlServer.Persistence.EntitiesRepositories;

public class BaseRepository<T> : IBaseRepository<T> where T : EntityBase
{
    private readonly AppDbContext _db;
    internal DbSet<T> _dbSet;

    public BaseRepository(AppDbContext db)
    {
        _db = db;
        _dbSet = _db.Set<T>();
    }

    public T? Find(int id)
    {
        return _dbSet.Find(id);
    }

    public async Task<T?> FindAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(id, cancellationToken);
    }

    public T? FirstOrDefault(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool isTracking = true)
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
            query = query.Where(filter);

        if (includeProperties != null)
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProp);

        if (!isTracking)
            query = query.AsNoTracking();

        return query.FirstOrDefault();
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool isTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;

        if (includeProperties != null)
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProp);

        if (filter != null)
            query = query.Where(filter);

        if (!isTracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null, bool isTracking = true)
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
            query = query.Where(filter);

        if (includeProperties != null)
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProp);

        if (orderBy != null)
            query = orderBy(query);

        if (!isTracking)
            query = query.AsNoTracking();

        return query.ToList();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null, bool isTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
            query = query.Where(filter);

        if (includeProperties != null)
            foreach (var includeProp in includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProp);

        if (orderBy != null)
            query = orderBy(query);

        if (!isTracking)
            query = query.AsNoTracking();

        return await query.ToListAsync(cancellationToken);
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public void AddBulk(IEnumerable<T> entityes)
    {
        _dbSet.AddRange(entityes);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entity)
    {
        _dbSet.RemoveRange(entity);
    }
}
