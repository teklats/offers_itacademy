// Copyright (C) TBC Bank. All Rights Reserved.

using MediatR;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Companies.Offers.Queries.GetCompanyOfferPurchases;

public class GetCompanyOfferPurchasesQuery : IRequest<List<PurchaseDto>>
{
    public Guid CompanyId { get; }

    public GetCompanyOfferPurchasesQuery(Guid companyId)
    {
        CompanyId = companyId;
    }
}
