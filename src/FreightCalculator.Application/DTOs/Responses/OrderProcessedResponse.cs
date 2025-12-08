namespace FreightCalculator.Application.DTOs.Responses;

public record OrderProcessedResponse(Guid OrderId, decimal ShippingCost);