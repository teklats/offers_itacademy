// Copyright (C) TBC Bank. All Rights Reserved.

using AutoMapper;
using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Companies.Offers.Queries.GetCompanyOfferPurchases;

public class GetCompanyOfferPurchasesQueryHandler : IRequestHandler<GetCompanyOfferPurchasesQuery, List<PurchaseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCompanyOfferPurchasesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<PurchaseDto>> Handle(GetCompanyOfferPurchasesQuery request, CancellationToken cancellationToken)
    {
        var offers = await _unitOfWork.OfferRepository.GetCompanyOffersAsync(request.CompanyId, cancellationToken);
        var offerIds = offers.Select(o => o.Id).ToList();
        var purchases = await _unitOfWork.PurchaseRepository.GetByOfferIdsAsync(offerIds, cancellationToken);
        return _mapper.Map<List<PurchaseDto>>(purchases);
    }
}
