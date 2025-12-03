namespace FreightCalculator.API.DTOs.Requests;

public record CreateOrderItemRequest(string ProductName, decimal Price, decimal Weight, int Quantity);