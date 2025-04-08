using AutoMapper;
using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Companies.Offers.Queries.GetCompanyOffers;

public class GetAllOfferQueryHandler : IRequestHandler<GetAllOfferQuery, IEnumerable<OfferDto>>
{
    private readonly IOfferRepository _offerRepository;
    private readonly IMapper _mapper;

    public GetAllOfferQueryHandler(IOfferRepository offerRepository, IMapper mapper)
    {
        _offerRepository = offerRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OfferDto>> Handle(GetAllOfferQuery request, CancellationToken cancellationToken)
    {
        var offers = await _offerRepository.GetAllOffersAsync(request.CompanyId,cancellationToken);
        return _mapper.Map<IEnumerable<OfferDto>>(offers);
    }
}