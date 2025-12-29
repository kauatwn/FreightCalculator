using FreightCalculator.Domain.Configuration;
using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Enums;
using FreightCalculator.Domain.Services.Shipping;
using Microsoft.Extensions.Options;

namespace FreightCalculator.UnitTests.Domain.Services.Shipping;

[Trait("Category", "Unit")]
public class ExpressShippingServiceTests
{
    private const decimal ExpressCostPerKg = 2.50m;

    private readonly ExpressShippingService _sut;

    public ExpressShippingServiceTests()
    {
        ShippingSettings settings = new() { ExpressCostPerKg = ExpressCostPerKg };
        IOptions<ShippingSettings> options = Options.Create(settings);

        _sut = new ExpressShippingService(options);
    }

    [Fact(DisplayName = "CalculateShippingCost should calculate based on weight")]
    public void CalculateShippingCost_ShouldCalculateBasedOnWeight_WhenOrderHasItems()
    {
        // Arrange
        List<OrderItem> items =
        [
            new OrderItem(productName: "Item A", price: 10.00m, weightInKg: 2m, quantity: 2),
            new OrderItem(productName: "Item B", price: 10.00m, weightInKg: 3m, quantity: 1)
        ];

        Order order = new(customerName: "Test", shippingMethod: ShippingMethod.Express, items: items);

        decimal expectedCost = 17.50m;

        // Act
        decimal cost = _sut.CalculateShippingCost(order);

        // Assert
        Assert.Equal(expectedCost, cost);
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