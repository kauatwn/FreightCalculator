using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Interfaces;

namespace FreightCalculator.Domain.Services.Shipping;

public class StandardShippingService : IShippingService
{
    private const decimal FixedFee = 10.00m;

    public decimal CalculateShippingCost(Order order)
    {
        return FixedFee;
    }
}