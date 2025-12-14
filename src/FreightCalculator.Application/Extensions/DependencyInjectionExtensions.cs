using FreightCalculator.Application.UseCases.Orders.Create;
using Microsoft.Extensions.DependencyInjection;

namespace FreightCalculator.Application.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICreateOrderUseCase, CreateOrderUseCase>();

        return services;
    }
}