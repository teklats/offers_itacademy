using AutoMapper;
using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Application.Features.Companies.Offers.Commands.CreateOffer;

public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, OfferResultDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOfferCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OfferResultDto> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
    {
        var companyIsActive = await _unitOfWork.CompanyRepository
            .CompanyIsActiveAsync(request.CompanyId, cancellationToken)
            .ConfigureAwait(false);

        if (!companyIsActive)
            throw new ForbiddenException("Company is not active");

        var offer = new Offer
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            UnitPrice = request.UnitPrice,
            InitialQuantity = request.InitialQuantity,
            AvailableQuantity = request.InitialQuantity,
            CreatedAt = DateTime.Now,
            ExpiresAt = request.ExpiresAt,
            CategoryId = request.CategoryId,
            CompanyId = request.CompanyId,
            ImageUrl = request.ImageUrl,
            Status = OfferStatus.Active
        };

        await _unitOfWork.OfferRepository.AddAsync(offer, cancellationToken).ConfigureAwait(false);
        await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);

        return _mapper.Map<OfferResultDto>(offer);
    }
}
