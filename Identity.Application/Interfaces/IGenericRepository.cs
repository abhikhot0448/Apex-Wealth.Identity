namespace Identity.Application.Interfaces;

public interface IGenericRepository<T>
    where T : class
{
    Task<T?> GetByIdAsync(Guid id);

    Task AddAsync(T entity);

    void Update(T entity);

    void Delete(T entity);
}
