using AutoMapper;
using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;

namespace OffersPlatform.Application.Features.Companies.Offers.Queries.GetOfferById;

public class GetOfferByIdQueryHandler : IRequestHandler<GetOfferByIdQuery, OfferDto>
{
    private readonly IOfferRepository _offerRepository;
    private readonly IMapper _mapper;

    public GetOfferByIdQueryHandler(IOfferRepository offerRepository, IMapper mapper)
    {
        _offerRepository = offerRepository;
        _mapper = mapper;
    }

    public async Task<OfferDto> Handle(GetOfferByIdQuery request, CancellationToken cancellationToken)
    {
        var offer = await _offerRepository
            .GetByIdAsync(request.OfferId, request.CompanyId, cancellationToken)
            .ConfigureAwait(false);
        if (offer is null)
        {
            throw new NotFoundException("Offer Not Found");
        }
        return _mapper.Map<OfferDto>(offer);
    }
}
