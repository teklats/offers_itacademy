using MediatR;
using OffersPlatform.Application.Common.Interfaces;
using OffersPlatform.Application.Exceptions;
using OffersPlatform.Application.Features.Companies.Offers.Commands.CancelOffer;
using OffersPlatform.Domain.Enums;

public class CancelOfferCommandHandler : IRequestHandler<CancelOfferCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public CancelOfferCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CancelOfferCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            var offer = await _unitOfWork.OfferRepository
                .GetByIdAsync(request.OfferId, request.CompanyId, cancellationToken).ConfigureAwait(false);
            if (offer == null)
                throw new NotFoundException("Offer not found.");

            if ((DateTime.UtcNow - offer.CreatedAt).TotalMinutes > 10)
                throw new Exception("You can only cancel an offer within 10 minutes.");

            var company = await _unitOfWork.CompanyRepository
                .GetCompanyByIdAsync(request.CompanyId, cancellationToken).ConfigureAwait(false);
            if (company == null)
                throw new NotFoundException("Company not found.");

            var purchases = await _unitOfWork.PurchaseRepository
                .GetByOfferIdAsync(request.OfferId, cancellationToken).ConfigureAwait(false);

            decimal totalRefund = 0;

            foreach (var purchase in purchases.Where(p => p.Status == PurchaseStatus.Completed))
            {
                var buyer = await _unitOfWork.UserRepository
                    .GetActiveUserByIdAsync(purchase.UserId, cancellationToken).ConfigureAwait(false);

                if (buyer != null)
                {
                    buyer.Balance += purchase.TotalPrice;
                    totalRefund += purchase.TotalPrice;

                    await _unitOfWork.UserRepository.UpdateAsync(buyer, cancellationToken).ConfigureAwait(false);
                }

                purchase.Status = PurchaseStatus.Cancelled;
                purchase.CancelledAt = DateTime.UtcNow;

                await _unitOfWork.PurchaseRepository.UpdateAsync(purchase, cancellationToken).ConfigureAwait(false);
            }

            company.Balance -= totalRefund;
            await _unitOfWork.CompanyRepository.UpdateAsync(company, cancellationToken).ConfigureAwait(false);

            offer.Status = OfferStatus.Canceled;
            await _unitOfWork.OfferRepository.UpdateAsync(offer, cancellationToken).ConfigureAwait(false);

            await _unitOfWork.CommitAsync(cancellationToken).ConfigureAwait(false);
            return true;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken).ConfigureAwait(false);
            throw;
        }
    }
}
