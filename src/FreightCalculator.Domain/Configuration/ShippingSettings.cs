using System.ComponentModel.DataAnnotations;

namespace FreightCalculator.Domain.Configuration;

public class ShippingSettings
{
    public const string SectionName = "Shipping";

    [Range(0.01, 1000)]
    public decimal ExpressCostPerKg { get; init; }

    [Range(0, 1000)]
    public decimal StandardFixedFee { get; init; }
}