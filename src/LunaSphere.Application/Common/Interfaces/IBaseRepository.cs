using LunaSphere.Domain.Common;

namespace LunaSphere.Application.Common.Interfaces;

/// <summary>
/// Base repository interface.
/// This include basic CRUD methods.
/// </summary>
public interface IBaseRepository<T> where T : Entity
{
    Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize);

    Task<T> GetByIdAsync(int id);

    Task AddAsync(T entity);

    void Update(T entity);

    Task DeleteAsync(int id);
}