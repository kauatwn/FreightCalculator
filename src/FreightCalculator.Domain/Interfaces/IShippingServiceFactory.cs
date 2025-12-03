using FreightCalculator.Domain.Enums;

namespace FreightCalculator.Domain.Interfaces;

public interface IShippingServiceFactory
{
    IShippingService GetService(ShippingMethod method);
}