using System.ComponentModel.DataAnnotations;

namespace FreightCalculator.Domain.Configuration;

public sealed class ShippingSettings
{
    public const string SectionName = "Shipping";

    [Range(0.1, double.MaxValue, ErrorMessage = "Express cost must be greater than zero.")]
    public decimal ExpressCostPerKg { get; init; }

    [Range(0, double.MaxValue, ErrorMessage = "Standard fee must be non-negative.")]
    public decimal StandardFixedFee { get; init; }

    [Range(0, double.MaxValue, ErrorMessage = "Free shipping threshold must be non-negative.")]
    public decimal FreeShippingThreshold { get; init; }
}