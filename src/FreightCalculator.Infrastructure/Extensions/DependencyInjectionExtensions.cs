using FreightCalculator.Domain.Configuration;
using FreightCalculator.Domain.Enums;
using FreightCalculator.Domain.Interfaces;
using FreightCalculator.Domain.Services.Shipping;
using FreightCalculator.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

        services.AddSingleton(provider => provider.GetRequiredService<IOptions<ShippingSettings>>().Value);
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddKeyedTransient<IShippingService, StandardShippingService>(ShippingMethod.Standard);
        services.AddKeyedTransient<IShippingService, ExpressShippingService>(ShippingMethod.Express);

        services.AddScoped<IShippingServiceFactory, ShippingServiceFactory>(provider =>
        {
            return new ShippingServiceFactory(method => provider.GetRequiredKeyedService<IShippingService>(method));
        });
    }
}