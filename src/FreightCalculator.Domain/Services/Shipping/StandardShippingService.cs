using FreightCalculator.Domain.Configuration;
using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace FreightCalculator.Domain.Services.Shipping;

public class StandardShippingService(IOptions<ShippingSettings> options) : IShippingService
{
    private readonly decimal _fixedFee = options.Value.StandardFixedFee;

    public decimal CalculateShippingCost(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        return _fixedFee;
    }
}