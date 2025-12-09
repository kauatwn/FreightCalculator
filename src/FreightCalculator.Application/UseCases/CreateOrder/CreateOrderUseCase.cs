using FreightCalculator.Application.DTOs.Requests;
using FreightCalculator.Application.DTOs.Responses;
using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FreightCalculator.Application.UseCases.CreateOrder;

public sealed partial class CreateOrderUseCase(IShippingServiceFactory shippingFactory, ILogger<CreateOrderUseCase> logger) : ICreateOrderUseCase
{
    private const int ProcessingStarted = 1;
    private const int ShippingCalculated = 2;
    private const int OrderProcessed = 3;

    public OrderProcessedResponse Execute(CreateOrderRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Items is null || request.Items.Count == 0)
        {
            throw new ArgumentException("The order must contain at least one item.", nameof(request));
        }

        Order order = new(request.CustomerName, request.ShippingMethod);

        foreach (CreateOrderItemRequest item in request.Items)
        {
            order.AddItem(new OrderItem(item.ProductName, item.Price, item.WeightInKg, item.Quantity));
        }

        LogProcessingStarted(order.Id, order.CustomerName);

        IShippingService shippingService = shippingFactory.GetService(order.ShippingMethod);
        decimal shippingCost = shippingService.CalculateShippingCost(order);

        LogShippingCalculated(order.Id, shippingCost);
        LogOrderProcessed(order.Id, shippingCost);

        return new OrderProcessedResponse(order.Id, shippingCost);
    }

    [LoggerMessage(EventId = ProcessingStarted, Level = LogLevel.Information, Message = "Starting processing for order {OrderId} requested by {CustomerName}")]
    private partial void LogProcessingStarted(Guid orderId, string customerName);

    [LoggerMessage(EventId = ShippingCalculated, Level = LogLevel.Information, Message = "Shipping cost calculated for order {OrderId}: {Cost}")]
    private partial void LogShippingCalculated(Guid orderId, decimal cost);

    [LoggerMessage(EventId = OrderProcessed, Level = LogLevel.Information, Message = "Order {OrderId} processed successfully with final cost {Cost}")]
    private partial void LogOrderProcessed(Guid orderId, decimal cost);
}