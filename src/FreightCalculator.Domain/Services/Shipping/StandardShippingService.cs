using FreightCalculator.Domain.Configuration;
using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace FreightCalculator.Domain.Services.Shipping;

public sealed class StandardShippingService(IOptions<ShippingSettings> options) : IShippingService
{
    private readonly decimal _fixedFee = options.Value.StandardFixedFee;
    private readonly decimal _freeShippingThreshold = options.Value.FreeShippingThreshold;

    public decimal CalculateShippingCost(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        if (order.Total >= _freeShippingThreshold)
        {
            return 0m;
        }

        return _fixedFee;
    }
}