namespace OffersPlatform.Application.Common.Interfaces.IRepositories;

public interface IRepository<T> where T : class
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<TDto?> GetByIdAsync<TDto>(Guid id, CancellationToken cancellationToken = default) where TDto : class;
    Task<IEnumerable<TDto?>> GetAllAsync<TDto>(CancellationToken cancellationToken = default) where TDto : class;
    void Update(T entity);
    void Delete(T entity);
    Task SaveAsync(CancellationToken cancellationToken = default);
}
