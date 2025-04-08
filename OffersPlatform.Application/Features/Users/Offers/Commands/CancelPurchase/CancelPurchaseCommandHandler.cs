using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Application.Features.Users.Offers.Commands.CancelPurchase;

public class CancelPurchaseCommandHandler : IRequestHandler<CancelPurchaseCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public CancelPurchaseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CancelPurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = await ValidatePurchase(request.PurchaseId, request.UserId, cancellationToken).ConfigureAwait(false);
        var user = await ValidateUser(request.UserId, cancellationToken).ConfigureAwait(false);
        var offer = await ValidateOffer(purchase.OfferId, cancellationToken).ConfigureAwait(false);
        var company = await ValidateCompany(offer.CompanyId, cancellationToken).ConfigureAwait(false);

        await CancelPurchase(purchase, offer, user, company, cancellationToken)
            .ConfigureAwait(false);

        await _unitOfWork
            .CommitAsync(cancellationToken)
            .ConfigureAwait(false);

        return true;
    }

    private async Task<Purchase> ValidatePurchase(Guid purchaseId, Guid userId, CancellationToken cancellationToken)
    {
        var purchase = await _unitOfWork.PurchaseRepository
            .GetByIdAsync(purchaseId, cancellationToken)
            .ConfigureAwait(false);
        if (purchase is null || purchase.UserId != userId)
            throw new NotFoundException("Purchase Not Found");

        if (purchase.PurchasedAt.AddMinutes(5) < DateTime.UtcNow)
            throw new Exception("Purchase can only be canceled within 5 minutes.");

        return purchase;
    }

    private async Task<User> ValidateUser(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository
            .GetActiveUserByIdAsync(userId, cancellationToken)
            .ConfigureAwait(false);
        if (user == null)
            throw new NotFoundException("User Not Found");

        return user;
    }

    private async Task<Offer> ValidateOffer(Guid offerId, CancellationToken cancellationToken)
    {
        var offer = await _unitOfWork.OfferRepository
            .GetByIdAsync(offerId, cancellationToken)
            .ConfigureAwait(false);
        if (offer == null)
            throw new NotFoundException("Offer Not Found");

        return offer;
    }

    private async Task<Company> ValidateCompany(Guid companyId, CancellationToken cancellationToken)
    {
        var company = await _unitOfWork.CompanyRepository
            .GetCompanyByIdAsync(companyId, cancellationToken)
            .ConfigureAwait(false);
        if (company == null)
            throw new NotFoundException("Company Not Found");

        return company;
    }

    private async Task CancelPurchase(Purchase purchase, Offer offer, User user, Company company, CancellationToken cancellationToken)
    {
        user.Balance += purchase.TotalPrice;

        offer.AvailableQuantity += purchase.Quantity;

        company.Balance -= purchase.TotalPrice;

        purchase.Status = PurchaseStatus.Cancelled;
        purchase.CancelledAt = DateTime.UtcNow;

        await _unitOfWork.PurchaseRepository
            .UpdateAsync(purchase, cancellationToken)
            .ConfigureAwait(false);
        await _unitOfWork.OfferRepository
            .UpdateAsync(offer, cancellationToken)
            .ConfigureAwait(false);
        await _unitOfWork.UserRepository
            .UpdateAsync(user, cancellationToken)
            .ConfigureAwait(false);
        _unitOfWork.CompanyRepository.UpdateAsync(company);
    }
}
