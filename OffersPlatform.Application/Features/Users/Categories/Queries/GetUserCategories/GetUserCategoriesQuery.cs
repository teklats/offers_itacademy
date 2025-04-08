using MediatR;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Users.Categories.Queries.GetUserCategories;

public class GetUserCategoriesQuery : IRequest<IEnumerable<CategoryDto>>
{
    public Guid UserId { get; set; }

    public GetUserCategoriesQuery(Guid userId)
    {
        UserId = userId;
    }
}