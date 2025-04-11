// Copyright (C) TBC Bank. All Rights Reserved.

namespace OffersPlatform.Application.Common.Interfaces;

public interface IDependencyHealthChecker
{
    Task<bool> CanConnectToDatabaseAsync(CancellationToken cancellationToken = default);
}
