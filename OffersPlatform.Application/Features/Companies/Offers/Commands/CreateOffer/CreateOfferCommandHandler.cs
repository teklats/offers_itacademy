using MediatR;
using OffersPlatform.Application.Common.Interfaces.IRepositories;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Application.Features.Companies.Offers.Commands.CreateOffer;

public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, Offer>
{
    private readonly IOfferRepository _offerRepository;

    public CreateOfferCommandHandler(IOfferRepository offerRepository)
    {
        _offerRepository = offerRepository;
    }

    public async Task<Offer> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = new Offer
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            UnitPrice = request.UnitPrice,
            InitialQuantity = request.InitialQuantity,
            AvailableQuantity = request.AvailableQuantity,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = request.ExpiresAt,
            Status = OfferStatus.Active,
            CategoryId = request.CategoryId,
            CompanyId = request.CompanyId,
        };

        await _offerRepository
            .AddAsync(offer, cancellationToken)
            .ConfigureAwait(false);
        return offer;
    }
}
