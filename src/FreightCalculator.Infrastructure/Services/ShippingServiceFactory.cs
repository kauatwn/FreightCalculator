using FreightCalculator.Domain.Enums;
using FreightCalculator.Domain.Interfaces;

namespace FreightCalculator.Infrastructure.Services;

public sealed class ShippingServiceFactory(Func<ShippingMethod, IShippingService> resolver) : IShippingServiceFactory
{
    public IShippingService GetService(ShippingMethod method) => resolver(method);
}