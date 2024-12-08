using Microsoft.EntityFrameworkCore;

using LunaSphere.Application.Common.Interfaces;
using LunaSphere.Domain.Common;

namespace LunaSphere.Infrastructure.Common.Persistence;

/// <summary>
/// Base Repository
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected DbSet<T> _entity;

    public BaseRepository(ApplicationDbContext context)
    {
        _entity = context.Set<T>();
    }

    /// <summary>
    /// Add entity asynchronous
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public async Task AddAsync(T entity)
    {
        await _entity.AddAsync(entity);
    }

    /// <summary>
    /// Delete entity using soft deletion
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(int id)
    {
        T entity = await GetByIdAsync(id);
        entity.IsActive = false;

        _entity.Update(entity);
    }

    /// <summary>
    /// Get all entities asynchronous
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<T>> GetAllAsync(int page = 1, int pageSize = 10)
    {
        return await _entity
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    /// <summary>
    /// Get entity by ID asynchronous
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<T> GetByIdAsync(int id)
    {
        var entity = await _entity.FirstOrDefaultAsync(e => e.Id == id);
        if (entity == null)
        {
            throw new KeyNotFoundException($"Entity with Id {id} does not exist.");
        }

        return entity;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    /// <param name="entity"></param>
    public void Update(T entity)
    {
        _entity.Update(entity);
    }
}
