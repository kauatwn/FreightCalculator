using FreightCalculator.Domain.Enums;
using FreightCalculator.Domain.Interfaces;

namespace FreightCalculator.Infrastructure.Services;

public class ShippingServiceFactory(Func<ShippingMethod, IShippingService> resolver) : IShippingServiceFactory
{
    public IShippingService GetService(ShippingMethod method) => resolver(method);
}