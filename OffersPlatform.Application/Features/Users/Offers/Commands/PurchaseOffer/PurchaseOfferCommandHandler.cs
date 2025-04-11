using AutoMapper;
using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Domain.Entities;
using OffersPlatform.Domain.Enums;

namespace OffersPlatform.Application.Features.Users.Offers.Commands.PurchaseOffer;
public class PurchaseOfferCommandHandler : IRequestHandler<PurchaseOfferCommand, PurchaseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PurchaseOfferCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PurchaseDto> Handle(PurchaseOfferCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Start a transaction
            // await _unitOfWork.BeginTransactionAsync(cancellationToken);

            // Load all necessary entities first
            var offer = await _unitOfWork.OfferRepository
                .GetByIdAsync(request.OfferId, cancellationToken)
                .ConfigureAwait(false);
            var user = await _unitOfWork.UserRepository
                .GetActiveUserByIdAsync(request.UserId, cancellationToken)
                .ConfigureAwait(false);
            var company = await _unitOfWork.CompanyRepository
                .GetCompanyByIdAsync(offer?.CompanyId ?? Guid.Empty, cancellationToken)
                .ConfigureAwait(false);

            // Validate offer and user
            ValidateOffer(offer, request.Quantity);
            ValidateUser(user, offer.UnitPrice * request.Quantity);
            ValidateCompany(company);

            // Calculate the total price
            decimal totalPrice = offer.UnitPrice * request.Quantity;

            // Update entities
            offer.AvailableQuantity -= request.Quantity;
            user.Balance -= totalPrice;
            company.Balance += totalPrice;

            // Create purchase
            var purchase = new Purchase
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                OfferId = offer.Id,
                Quantity = request.Quantity,
                TotalPrice = totalPrice,
                Status = PurchaseStatus.Completed,
                PurchasedAt = DateTime.Now
            };

            // Save changes in a single transaction
            await _unitOfWork.PurchaseRepository
                .AddAsync(purchase, cancellationToken)
                .ConfigureAwait(false);
            await _unitOfWork.OfferRepository
                .UpdateAsync(offer, cancellationToken)
                .ConfigureAwait(false);
            await _unitOfWork.UserRepository
                .UpdateAsync(user, cancellationToken)
                .ConfigureAwait(false);
            await _unitOfWork
                .CompanyRepository.UpdateAsync(company, cancellationToken).ConfigureAwait(false);

            // Commit all changes
            await _unitOfWork
                .CommitAsync(cancellationToken)
                .ConfigureAwait(false);

            return _mapper.Map<PurchaseDto>(purchase);
        }
        catch (Exception ex)
        {
            // Rollback on any exception
            // await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw new Exception($"Failed to purchase offer: {ex.Message}", ex);
        }
    }

    private static void ValidateOffer(Offer? offer, int quantity)
    {
        if (offer is null || offer.Status != OfferStatus.Active || offer.ExpiresAt <= DateTime.Now)
            throw new Exception("Offer is not available");

        if (offer.AvailableQuantity < quantity)
            throw new Exception("Not enough quantity available");
    }

    private static void ValidateUser(User? user, decimal totalPrice)
    {
        if (user == null || user.Balance < totalPrice)
            throw new Exception("User does not have enough balance");
    }

    private static void ValidateCompany(Company? company)
    {
        if (company == null)
            throw new NotFoundException("Company Not Found");
    }
}
