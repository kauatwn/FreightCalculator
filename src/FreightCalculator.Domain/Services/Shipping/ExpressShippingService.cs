using FreightCalculator.Domain.Configuration;
using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Interfaces;

namespace FreightCalculator.Domain.Services.Shipping;

public sealed class ExpressShippingService(ShippingSettings settings) : IShippingService
{
    private readonly decimal _costPerKg = settings.ExpressCostPerKg;

    public decimal CalculateShippingCost(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);

        return order.Items.Sum(i => i.WeightInKg * i.Quantity) * _costPerKg;
    }
}