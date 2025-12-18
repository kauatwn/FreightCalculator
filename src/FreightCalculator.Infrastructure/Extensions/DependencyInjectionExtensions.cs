using FreightCalculator.Domain.Configuration;
using FreightCalculator.Domain.Enums;
using FreightCalculator.Domain.Interfaces;
using FreightCalculator.Domain.Services.Shipping;
using FreightCalculator.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace FreightCalculator.Infrastructure.Extensions;

[ExcludeFromCodeCoverage(Justification = "Pure dependency injection configuration")]
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddConfiguration(services, configuration);
        AddServices(services);

        return services;
    }

    private static void AddConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ShippingSettings>()
            .Bind(configuration.GetSection(ShippingSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddKeyedScoped<IShippingService, StandardShippingService>(ShippingMethod.Standard);
        services.AddKeyedScoped<IShippingService, ExpressShippingService>(ShippingMethod.Express);

        services.AddScoped<IShippingServiceFactory, ShippingServiceFactory>(provider =>
        {
            return new ShippingServiceFactory(method => provider.GetRequiredKeyedService<IShippingService>(method));
        });
    }
}