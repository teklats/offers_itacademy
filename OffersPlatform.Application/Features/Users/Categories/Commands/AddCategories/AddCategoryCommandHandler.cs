using AutoMapper;
using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;

namespace OffersPlatform.Application.Features.Users.Categories.Commands.AddCategories;

public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, UserCategoryDto>
{
    private readonly IUserCategoryRepository _userCategoryRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public AddCategoryCommandHandler(IUserCategoryRepository userCategoryRepository,
        ICategoryRepository categoryRepository, IMapper mapper)
    {
        _userCategoryRepository = userCategoryRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<UserCategoryDto> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        var exists = await _categoryRepository
            .GetByIdAsync(request.CategoryId, cancellationToken)
            .ConfigureAwait(false);
        if (exists == null)
            throw new NotFoundException("Category Not Found");

        var existing = await _userCategoryRepository
            .GetByUserIdAndCategoryIdAsync(request.UserId, request.CategoryId, cancellationToken)
            .ConfigureAwait(false);
        if (existing != null)
            throw new AlreadyExistsException("Category Already In Preference");

        var userCategory = await _userCategoryRepository
            .AddCategoryToPreferenceAsync(request.UserId, request.CategoryId, cancellationToken)
            .ConfigureAwait(false);

        return _mapper.Map<UserCategoryDto>(userCategory);
    }

}
