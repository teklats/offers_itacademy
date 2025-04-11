// Copyright (C) TBC Bank. All Rights Reserved.

namespace OffersPlatform.Application.DTOs;

public class HealthStatus
{
    public string Status { get; set; } = "Unhealthy";
    public bool DatabaseConnected { get; set; }
    public DateTime CheckedAt { get; set; } = DateTime.Now;
    public string? Message { get; set; }
}
