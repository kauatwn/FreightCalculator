using FreightCalculator.Application.Services;
using FreightCalculator.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FreightCalculator.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}