namespace FreightCalculator.Application.DTOs.Responses;

public sealed record OrderProcessedResponse(Guid OrderId, decimal ShippingCost);