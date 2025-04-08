using OffersPlatform.Application.DTOs;
using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Common.Interfaces.IRepositories;

public interface ICompanyRepository : IRepository<Company>
{
    Task<Company?> GetCompanyByEmailAsync(string? email, CancellationToken cancellationToken = default);
    Task<string> GetPasswordHashAsync(string? email, CancellationToken cancellationToken);
    Task AddAsync(Company company, CancellationToken cancellationToken = default);
    Task UpdateAsync(Company company, CancellationToken cancellationToken = default);
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Company?>> GetAllActiveCompaniesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Company?>> GetAllCompaniesAsync(CancellationToken cancellationToken = default);
    Task<Company?> GetCompanyByIdAsync(Guid id, CancellationToken cancellationToken = default);
}