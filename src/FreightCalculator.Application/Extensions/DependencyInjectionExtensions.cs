using FreightCalculator.Application.UseCases.CreateOrder;
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