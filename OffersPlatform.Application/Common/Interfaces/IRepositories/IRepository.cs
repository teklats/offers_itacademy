namespace OffersPlatform.Application.Common.Interfaces.IRepositories;

public interface IRepository<T> where T : class
{
    // Task AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Delete(T entity);
    Task SaveAsync(CancellationToken cancellationToken = default);
}
