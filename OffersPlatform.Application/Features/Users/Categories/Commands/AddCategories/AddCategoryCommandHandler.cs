using AutoMapper;
using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;

namespace OffersPlatform.Application.Features.Users.Categories.Commands.AddCategories;

public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, UserCategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserCategoryDto> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        var exists = await _unitOfWork.CategoryRepository
            .GetByIdAsync(request.CategoryId, cancellationToken)
            .ConfigureAwait(false);

        if (exists == null)
            throw new NotFoundException("Category Not Found");

        var existing = await _unitOfWork.UserCategoryRepository
            .GetByUserIdAndCategoryIdAsync(request.UserId, request.CategoryId, cancellationToken)
            .ConfigureAwait(false);

        if (existing != null)
            throw new AlreadyExistsException("Category Already In Preference");

        var userCategory = await _unitOfWork.UserCategoryRepository
            .AddCategoryToPreferenceAsync(request.UserId, request.CategoryId, cancellationToken)
            .ConfigureAwait(false);

        await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);

        return _mapper.Map<UserCategoryDto>(userCategory);
    }
}
