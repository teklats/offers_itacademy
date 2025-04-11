// Copyright (C) TBC Bank. All Rights Reserved.
using MediatR;
namespace OffersPlatform.Application.Features.Companies.Profile.Commands;

public record UploadCompanyImageCommand(Guid CompanyId, string ImageUrl) : IRequest;
