using FreightCalculator.Domain.Configuration;
using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Enums;
using FreightCalculator.Domain.Services.Shipping;
using Microsoft.Extensions.Options;

namespace FreightCalculator.UnitTests.Domain.Services.Shipping;

[Trait("Category", "Unit")]
public class ExpressShippingServiceTests
{
    private readonly ExpressShippingService _sut;

    public ExpressShippingServiceTests()
    {
        ShippingSettings settings = new() { ExpressCostPerKg = 2.50m };
        IOptions<ShippingSettings> options = Options.Create(settings);

        _sut = new ExpressShippingService(options);
    }

    [Fact(DisplayName = "CalculateShippingCost should calculate based on weight")]
    public void CalculateShippingCost_ShouldCalculateBasedOnWeight_WhenOrderHasItems()
    {
        // Arrange
        Order order = new(customerName: "Test", shippingMethod: ShippingMethod.Express);

        order.AddItem(new OrderItem(productName: "Item A", price: 10.00m, weightInKg: 2m, quantity: 2));
        order.AddItem(new OrderItem(productName: "Item B", price: 10.00m, weightInKg: 3m, quantity: 1));

        // Act
        decimal cost = _sut.CalculateShippingCost(order);

        // Assert
        Assert.Equal(17.50m, cost);
    }

    [Fact(DisplayName = "CalculateShippingCost should return zero when order is empty")]
    public void CalculateShippingCost_ShouldReturnZero_WhenOrderHasNoItems()
    {
        // Arrange
        Order order = new(customerName: "Test", shippingMethod: ShippingMethod.Express);

        // Act
        decimal cost = _sut.CalculateShippingCost(order);

        // Assert
        Assert.Equal(0.00m, cost);
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