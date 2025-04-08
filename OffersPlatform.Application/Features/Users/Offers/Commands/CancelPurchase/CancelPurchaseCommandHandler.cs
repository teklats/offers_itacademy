using System.Net;
using AutoMapper;
using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Application.Features.Users.Offers.Commands.CancelPurchase;

public class CancelPurchaseCommandHandler : IRequestHandler<CancelPurchaseCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CancelPurchaseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> Handle(CancelPurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = await ValidatePurchase(request.PurchaseId, request.UserId, cancellationToken);
        var user = await ValidateUser(request.UserId, purchase.TotalPrice, cancellationToken);
        var offer = await ValidateOffer(purchase.OfferId, cancellationToken);
        var company = await ValidateCompany(offer.CompanyId, cancellationToken);

        await CancelPurchase(purchase, offer, user, company, cancellationToken);
        
        await _unitOfWork.CommitAsync(cancellationToken);

        return true;
    }

    private async Task<Purchase> ValidatePurchase(Guid purchaseId, Guid userId, CancellationToken cancellationToken)
    {
        var purchase = await _unitOfWork.PurchaseRepository.GetByIdAsync(purchaseId, cancellationToken);
        if (purchase is null || purchase.UserId != userId)
            throw new NotFoundException("Purchase Not Found");
        
        if (purchase.PurchasedAt.AddMinutes(5) < DateTime.UtcNow)
            throw new Exception("Purchase can only be canceled within 5 minutes.");

        return purchase;
    }

    private async Task<User> ValidateUser(Guid userId, decimal totalPrice, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetActiveUserByIdAsync(userId, cancellationToken);
        if (user == null)
            throw new NotFoundException("User Not Found");

        return user;
    }

    private async Task<Offer> ValidateOffer(Guid offerId, CancellationToken cancellationToken)
    {
        var offer = await _unitOfWork.OfferRepository.GetByIdAsync(offerId, cancellationToken);
        if (offer == null)
            throw new NotFoundException("Offer Not Found");

        return offer;
    }

    private async Task<Company> ValidateCompany(Guid companyId, CancellationToken cancellationToken)
    {
        var company = await _unitOfWork.CompanyRepository.GetCompanyByIdAsync(companyId, cancellationToken);
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
        
        await _unitOfWork.PurchaseRepository.UpdateAsync(purchase, cancellationToken);
        await _unitOfWork.OfferRepository.UpdateAsync(offer, cancellationToken);
        await _unitOfWork.UserRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.CompanyRepository.UpdateAsync(company, cancellationToken);
    }
}
