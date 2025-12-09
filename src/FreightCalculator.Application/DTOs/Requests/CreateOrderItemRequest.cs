namespace FreightCalculator.Application.DTOs.Requests;

public sealed record CreateOrderItemRequest(string ProductName, decimal Price, decimal WeightInKg, int Quantity);