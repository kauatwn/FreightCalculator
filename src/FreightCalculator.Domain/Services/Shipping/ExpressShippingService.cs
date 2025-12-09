using FreightCalculator.Domain.Configuration;
using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace FreightCalculator.Domain.Services.Shipping;

public sealed class ExpressShippingService(IOptions<ShippingSettings> options) : IShippingService
{
    private readonly decimal _costPerKg = options.Value.ExpressCostPerKg;

    public decimal CalculateShippingCost(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        return order.Items.Sum(i => i.WeightInKg * i.Quantity) * _costPerKg;
    }
}