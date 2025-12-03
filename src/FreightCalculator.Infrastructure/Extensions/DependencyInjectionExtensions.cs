using FreightCalculator.Domain.Enums;
using FreightCalculator.Domain.Interfaces;
using FreightCalculator.Domain.Services.Shipping;
using FreightCalculator.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FreightCalculator.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddKeyedScoped<IShippingService, StandardShippingService>(ShippingMethod.Standard);
        services.AddKeyedScoped<IShippingService, ExpressShippingService>(ShippingMethod.Express);

        services.AddScoped<IShippingServiceFactory, ShippingServiceFactory>(provider =>
        {
            return new ShippingServiceFactory(method => provider.GetRequiredKeyedService<IShippingService>(method));
        });

        return services;
    }
}