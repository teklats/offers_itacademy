using OffersPlatform.Application.DTOs;
using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Common.Interfaces.IRepositories;

public interface ICompanyRepository : IRepository<Company>
{
    Task<CompanyDto?> GetCompanyByEmailAsync(string? email, CancellationToken cancellationToken = default);
    Task AddAsync(Company company, CancellationToken cancellationToken = default);
    Task UpdateAsync(Company company, CancellationToken cancellationToken = default);
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompanyDto?>> GetAllActiveCompaniesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CompanyDto?>> GetAllCompaniesAsync(CancellationToken cancellationToken = default);
    Task<Company?> GetCompanyByIdAsync(Guid id, CancellationToken cancellationToken = default);
}