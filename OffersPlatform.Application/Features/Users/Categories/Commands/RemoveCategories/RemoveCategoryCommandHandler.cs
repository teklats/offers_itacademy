using System.Net;
using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.Exceptions;

namespace OffersPlatform.Application.Features.Users.Categories.Commands.RemoveCategories;

public class RemoveCategoryCommandHandler : IRequestHandler<RemoveCategoryCommand, bool>
{
    private readonly IUserCategoryRepository _userCategoryRepository;

    public RemoveCategoryCommandHandler(IUserCategoryRepository userCategoryRepository)
    {
        _userCategoryRepository = userCategoryRepository;
    }

    public async Task<bool> Handle(RemoveCategoryCommand request, CancellationToken cancellationToken)
    {
        var exist = await _userCategoryRepository.GetByUserIdAndCategoryIdAsync(request.UserId, request.CategoryId, cancellationToken);
        if (exist is null)
        {
            throw new NotFoundException("Category Not Found");
        }
        
        return await _userCategoryRepository.RemoveCategoryFromPreferenceAsync(request.UserId, request.CategoryId, cancellationToken);
    }
}