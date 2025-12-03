using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FreightCalculator.Application.Services;

public partial class OrderService(IShippingServiceFactory shippingFactory, ILogger<OrderService> logger) : IOrderService
{
    public void ProcessOrder(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        LogProcessingStarted(order.Id);

        IShippingService shippingService = shippingFactory.GetService(order.ShippingMethod);

        decimal shippingCost = shippingService.CalculateShippingCost(order);

        LogShippingCalculated(shippingCost, order.Id);
        LogProcessedSuccessfully(order.Id);
    }

    [LoggerMessage(LogLevel.Information, "Starting processing for order {Id}")]
    private partial void LogProcessingStarted(Guid id);

    [LoggerMessage(LogLevel.Information, "Shipping cost calculated: {Cost} for Order {Id}")]
    private partial void LogShippingCalculated(decimal cost, Guid id);

    [LoggerMessage(LogLevel.Information, "Order {Id} processed successfully")]
    private partial void LogProcessedSuccessfully(Guid id);
}