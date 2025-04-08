using AutoMapper;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Mapping;

public class UserCategoryMappingProfile : Profile
{
    public UserCategoryMappingProfile()
    {
        CreateMap<UserCategory, UserCategoryDto>()
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

        CreateMap<UserCategoryDto, UserCategory>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));
        
        CreateMap<UserCategory, CategoryDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Category.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Category.Description))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Category.CreatedAt));
    }
}