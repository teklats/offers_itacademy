// Copyright (C) TBC Bank. All Rights Reserved.

using MediatR;
using OffersPlatform.Application.DTOs;

namespace OffersPlatform.Application.Features.Auth.Commands.Login;
public class LoginCommand : IRequest<AuthDto?>
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}
