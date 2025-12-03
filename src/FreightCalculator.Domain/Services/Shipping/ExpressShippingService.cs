using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Interfaces;

namespace FreightCalculator.Domain.Services.Shipping;

public class ExpressShippingService : IShippingService
{
    private const decimal CostPerKg = 1.00m;

    public decimal CalculateShippingCost(Order order)
    {
        return order.Items.Sum(i => i.Weight) * CostPerKg;
    }
}