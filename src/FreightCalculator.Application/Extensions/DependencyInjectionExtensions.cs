using FreightCalculator.Application.UseCases.Orders.Create;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace FreightCalculator.Application.Extensions;

[ExcludeFromCodeCoverage(Justification = "Pure dependency injection configuration")]
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICreateOrderUseCase, CreateOrderUseCase>();

        return services;
    }
}