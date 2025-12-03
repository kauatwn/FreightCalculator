using FreightCalculator.Domain.Configuration;
using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace FreightCalculator.Domain.Services.Shipping;

public class ExpressShippingService(IOptions<ShippingSettings> options) : IShippingService
{
    private readonly decimal _costPerKg = options.Value.ExpressCostPerKg;

    public decimal CalculateShippingCost(Order order)
    {
        return order.Items.Sum(i => i.Weight) * _costPerKg;
    }
}