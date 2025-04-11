using AutoMapper;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Mapping;

public class PurchaseMappingProfile : Profile
{
    public PurchaseMappingProfile()
    {
        CreateMap<Purchase, PurchaseDto>()
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.OfferId, opt => opt.MapFrom(src => src.OfferId))
            .ForMember(dest => dest.PurchasedAt, opt => opt.MapFrom(src => src.PurchasedAt))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

        CreateMap<PurchaseDto, Purchase>()
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.OfferId, opt => opt.MapFrom(src => src.OfferId))
            .ForMember(dest => dest.PurchasedAt, opt => opt.MapFrom(src => src.PurchasedAt))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

    }
}
