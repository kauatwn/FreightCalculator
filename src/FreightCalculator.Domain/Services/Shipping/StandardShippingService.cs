using FreightCalculator.Domain.Configuration;
using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Interfaces;

namespace FreightCalculator.Domain.Services.Shipping;

public sealed class StandardShippingService(ShippingSettings settings) : IShippingService
{
    private readonly decimal _fixedFee = settings.StandardFixedFee;
    private readonly decimal _freeShippingThreshold = settings.FreeShippingThreshold;

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