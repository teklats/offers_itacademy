using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Common.Interfaces;

public interface IUserCategoryRepository
{
    Task<IEnumerable<UserCategory>> GetUserCategoriesAsync(Guid userId);
    Task AddUserCategoryAsync(Guid userId, Guid categoryId);
    Task RemoveUserCategoryAsync(Guid userId, Guid categoryId);
}