using MediatR;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Users.Categories.Commands.AddCategories;

public class AddCategoryCommand : IRequest<UserCategoryDto>
{
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }

    public AddCategoryCommand(Guid userId, Guid categoryId)
    {
        UserId = userId;
        CategoryId = categoryId;
    }
}