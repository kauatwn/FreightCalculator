namespace FreightCalculator.Application.DTOs.Requests;

public record CreateOrderItemRequest(string ProductName, decimal Price, decimal WeightInKg, int Quantity);