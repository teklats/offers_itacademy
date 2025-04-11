using MediatR;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Features.Companies.Offers.Commands.CreateOffer;


public class CreateOfferCommand : IRequest<OfferResultDto>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal UnitPrice { get; set; }
    public int InitialQuantity { get; set; }
    public int AvailableQuantity { get; set; }
    public DateTime ExpiresAt { get; set; }
    public Guid CategoryId { get; set; }
    public Guid CompanyId { get; set; }
    public string ImageUrl { get; set; }


    public CreateOfferCommand(OfferCreateDto request, Guid companyId)
    {
        Name = request.Name;
        Description = request.Description;
        UnitPrice = request.UnitPrice;
        InitialQuantity = request.InitialQuantity;
        AvailableQuantity = request.AvailableQuantity;
        ExpiresAt = request.ExpiresAt;
        CategoryId = request.CategoryId;
        ImageUrl = request.ImageUrl;
        CompanyId = companyId;
    }
}
