using FreightCalculator.Domain.Entities;

namespace FreightCalculator.Domain.Interfaces;

public interface IOrderService
{
    void ProcessOrder(Order order);
}