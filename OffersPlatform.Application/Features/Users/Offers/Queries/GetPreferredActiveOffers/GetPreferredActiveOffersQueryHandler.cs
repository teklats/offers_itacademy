using AutoMapper;
using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Users.Offers.Queries.GetPreferredActiveOffers;

public class GetPreferredActiveOffersQueryHandler : IRequestHandler<GetPreferredActiveOffersQuery, IEnumerable<OfferDto>>
{
    private readonly IOfferRepository _offerRepository;
    private readonly IMapper _mapper;
    private readonly IUserCategoryRepository _userCategoryRepository;

    public GetPreferredActiveOffersQueryHandler(IOfferRepository offerRepository, IMapper mapper,
        IUserCategoryRepository userCategoryRepository)
    {
        _offerRepository = offerRepository;
        _mapper = mapper;
        _userCategoryRepository = userCategoryRepository;
    }

    public async Task<IEnumerable<OfferDto>> Handle(GetPreferredActiveOffersQuery request,
        CancellationToken cancellationToken)
    {
        var userCategories = await _userCategoryRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        var categoryIds = userCategories.Select(uc => uc.CategoryId).ToList();

        if (!categoryIds.Any())
            return Enumerable.Empty<OfferDto>();

        var offers = await _offerRepository.GetByCategoryIdsAsync(categoryIds, cancellationToken);
        return _mapper.Map<IEnumerable<OfferDto>>(offers);
        
    }
}