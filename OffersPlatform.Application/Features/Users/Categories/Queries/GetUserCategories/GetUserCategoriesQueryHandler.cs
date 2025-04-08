using AutoMapper;
using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Users.Categories.Queries.GetUserCategories;

public class GetUserCategoriesQueryHandler : IRequestHandler<GetUserCategoriesQuery, IEnumerable<CategoryDto>>
{
    private readonly IUserCategoryRepository _userCategoryRepository;
    private readonly IMapper _mapper;

    public GetUserCategoriesQueryHandler(IUserCategoryRepository userCategoryRepository, IMapper mapper)
    {
        _userCategoryRepository = userCategoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDto>> Handle(GetUserCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var preferences = await _userCategoryRepository
            .GetByUserIdAsync(request.UserId, cancellationToken)
            .ConfigureAwait(false);
        return _mapper.Map<IEnumerable<CategoryDto>>(preferences);
    }
}
