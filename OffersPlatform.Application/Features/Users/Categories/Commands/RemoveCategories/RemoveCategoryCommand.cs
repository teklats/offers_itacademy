using MediatR;

namespace OffersPlatform.Application.Features.Users.Categories.Commands.RemoveCategories;

public class RemoveCategoryCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    

    public RemoveCategoryCommand(Guid userId, Guid categoryId)
    {
        UserId = userId;
        CategoryId = categoryId;
    }
}