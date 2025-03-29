using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Common.Interfaces;

public interface ICompanyRepository
{
    Task<Company?> GetByIdAsync(Guid id);
    Task<IEnumerable<Company>> GetPendingApprovalAsync();
    Task<IEnumerable<Company>> GetAllActiveAsync();
    Task AddAsync(Company company);
    Task UpdateAsync(Company company);
    Task ApproveCompanyAsync(Guid companyId);
}