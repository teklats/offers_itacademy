using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Application.Common.Interfaces;

public interface IAuthService
{
    string RegisterUser(User user, string password);
    string RegisterCompany(Company company, string password);
    string Login(UserRole role, Guid id);
}
