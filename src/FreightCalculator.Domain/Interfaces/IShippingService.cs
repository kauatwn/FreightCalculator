using FreightCalculator.Domain.Entities;

namespace FreightCalculator.Domain.Interfaces;

public interface IShippingService
{
    decimal CalculateShippingCost(Order order);
}