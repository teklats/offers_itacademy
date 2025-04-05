using MediatR;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Admin.Users.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<UserDto>
{
    public Guid Id { get; set; }

    public GetUserByIdQuery(Guid id)
    {
        Id = id;
    }
}