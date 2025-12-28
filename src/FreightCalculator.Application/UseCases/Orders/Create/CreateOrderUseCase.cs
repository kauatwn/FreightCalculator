using FreightCalculator.Application.DTOs.Requests;
using FreightCalculator.Application.DTOs.Responses;
using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Exceptions;
using FreightCalculator.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FreightCalculator.Application.UseCases.Orders.Create;

public sealed partial class CreateOrderUseCase(
    IShippingServiceFactory shippingFactory,
    ILogger<CreateOrderUseCase> logger) : ICreateOrderUseCase
{
    private const int ProcessingStarted = 1;
    private const int ShippingCalculated = 2;
    private const int OrderProcessed = 3;

    public const string ShippingMethodRequired = "Shipping method is required.";
    public const string OrderMustHaveItems = "The order must contain at least one item.";

    public OrderProcessedResponse Execute(CreateOrderRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.ShippingMethod is null)
        {
            throw new ValidationException(ShippingMethodRequired);
        }

        if (request.Items is null || request.Items.Count == 0)
        {
            throw new ValidationException(OrderMustHaveItems);
        }

        IEnumerable<OrderItem> items = request.Items.Select(i => new OrderItem(
            i.ProductName,
            i.Price,
            i.WeightInKg,
            i.Quantity));

        Order order = new(request.CustomerName, request.ShippingMethod.Value, items);

        LogProcessingStarted(order.Id, order.CustomerName);

        IShippingService shippingService = shippingFactory.GetService(order.ShippingMethod);
        decimal shippingCost = shippingService.CalculateShippingCost(order);

        LogShippingCalculated(order.Id, shippingCost);
        LogOrderProcessed(order.Id, shippingCost);

        return new OrderProcessedResponse(order.Id, shippingCost);
    }

    [LoggerMessage(
        EventId = ProcessingStarted,
        Level = LogLevel.Information,
        Message = "Starting processing for order {OrderId} requested by {CustomerName}")]
    private partial void LogProcessingStarted(Guid orderId, string customerName);

    [LoggerMessage(
        EventId = ShippingCalculated,
        Level = LogLevel.Information,
        Message = "Shipping cost calculated for order {OrderId}: {Cost}")]
    private partial void LogShippingCalculated(Guid orderId, decimal cost);

    [LoggerMessage(
        EventId = OrderProcessed,
        Level = LogLevel.Information,
        Message = "Order {OrderId} processed successfully with final cost {Cost}")]
    private partial void LogOrderProcessed(Guid orderId, decimal cost);
}