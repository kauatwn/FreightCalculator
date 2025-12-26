using System.ComponentModel.DataAnnotations;

namespace FreightCalculator.Domain.Configuration;

public sealed class ShippingSettings
{
    public const string SectionName = "Shipping";

    [Range(0.01, 1_000)]
    public decimal ExpressCostPerKg { get; init; }

    [Range(0, 1_000)]
    public decimal StandardFixedFee { get; init; }
}