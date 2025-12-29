using FreightCalculator.Domain.Configuration;
using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Enums;
using FreightCalculator.Domain.Services.Shipping;

namespace FreightCalculator.UnitTests.Domain.Services.Shipping;

[Trait("Category", "Unit")]
public class StandardShippingServiceTests
{
    private const decimal FixedFee = 10.00m;
    private const decimal FreeShippingThreshold = 200.00m;

    private readonly StandardShippingService _sut;

    public StandardShippingServiceTests()
    {
        ShippingSettings settings = new()
        {
            StandardFixedFee = FixedFee,
            FreeShippingThreshold = FreeShippingThreshold
        };

        _sut = new StandardShippingService(settings);
    }

    [Fact(DisplayName = "CalculateShippingCost should return fixed fee when total is below threshold")]
    public void CalculateShippingCost_ShouldReturnFixedFee_WhenTotalIsBelowThreshold()
    {
        // Arrange
        List<OrderItem> items =
        [
            new OrderItem(productName: "Item A", price: 10.00m, weightInKg: 0.5m, quantity: 5)
        ];

        Order order = new(customerName: "Test", ShippingMethod.Standard, items);

        // Act
        decimal cost = _sut.CalculateShippingCost(order);

        // Assert
        Assert.Equal(FixedFee, cost);
    }

    [Fact(DisplayName = "CalculateShippingCost should return zero when total is equal or above threshold")]
    public void CalculateShippingCost_ShouldReturnZero_WhenTotalIsAboveThreshold()
    {
        // Arrange
        List<OrderItem> items =
        [
            new OrderItem(productName: "Expensive Item", price: 100.00m, weightInKg: 1m, quantity: 3)
        ];

        Order order = new(customerName: "Test", ShippingMethod.Standard, items);

        // Act
        decimal cost = _sut.CalculateShippingCost(order);

        // Assert
        Assert.Equal(0m, cost);
    }

    [Fact(DisplayName = "CalculateShippingCost should throw ArgumentNullException when order is null")]
    public void CalculateShippingCost_ShouldThrowArgumentNullException_WhenOrderIsNull()
    {
        // Act
        void Act() => _sut.CalculateShippingCost(null!);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(Act);
        Assert.Equal("order", exception.ParamName);
    }
}