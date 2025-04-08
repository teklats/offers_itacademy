using AutoMapper;
using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Users.Offers.Queries.GetUserPurchase;

public class GetUserPurchaseHistoryQueryHandler : IRequestHandler<GetUserPurchaseHistoryQuery, IEnumerable<PurchaseDto>>
{
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IMapper _mapper;

    public GetUserPurchaseHistoryQueryHandler(IPurchaseRepository purchaseRepository, IMapper mapper)
    {
        _purchaseRepository = purchaseRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PurchaseDto>> Handle(GetUserPurchaseHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var purchases = await _purchaseRepository
            .GetByUserIdAsync(request.UserId, cancellationToken)
            .ConfigureAwait(false);
        return _mapper.Map<IEnumerable<PurchaseDto>>(purchases);
    }
}
