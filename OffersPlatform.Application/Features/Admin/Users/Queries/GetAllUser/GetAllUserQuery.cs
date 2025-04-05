using MediatR;
using OffersPlatform.Application.DTOs;
using OffersPlatform.Domain.Entities;

namespace OffersPlatform.Application.Features.Admin.Users.Queries.GetAllUser;

public class GetAllUserQuery : IRequest<IEnumerable<UserDto?>>
{
    
}