// Copyright (C) TBC Bank. All Rights Reserved.

using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Exceptions;

namespace OffersPlatform.Application.Features.Companies.Profile.Commands;

public class UploadCompanyImageCommandHandler : IRequestHandler<UploadCompanyImageCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UploadCompanyImageCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UploadCompanyImageCommand request, CancellationToken cancellationToken)
    {
        var company = await _unitOfWork.CompanyRepository
            .GetCompanyByIdAsync(request.CompanyId, cancellationToken)
            .ConfigureAwait(false);
        if (company == null)
            throw new NotFoundException("Company not found.");

        company.ImageUrl = request.ImageUrl;
        await _unitOfWork.CompanyRepository.UpdateAsync(company, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);

        return Unit.Value;
    }
}
