using System.Net;
using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Exceptions;

namespace OffersPlatform.Application.Features.Users.Categories.Commands.RemoveCategories;

public class RemoveCategoryCommandHandler : IRequestHandler<RemoveCategoryCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<bool> Handle(RemoveCategoryCommand request, CancellationToken cancellationToken)
    {
        var exist = await _unitOfWork.UserCategoryRepository
            .GetByUserIdAndCategoryIdAsync(request.UserId, request.CategoryId, cancellationToken)
            .ConfigureAwait(false);

        if (exist is null)
            throw new NotFoundException("Category Not Found");

        var result = await _unitOfWork.UserCategoryRepository
            .RemoveCategoryFromPreferenceAsync(request.UserId, request.CategoryId, cancellationToken)
            .ConfigureAwait(false);

        await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);

        return result;
    }

}
