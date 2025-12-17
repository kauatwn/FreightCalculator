using FreightCalculator.Domain.Configuration;
using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Enums;
using FreightCalculator.Domain.Services.Shipping;
using Microsoft.Extensions.Options;

namespace FreightCalculator.UnitTests.Domain.Services.Shipping;

[Trait("Category", "Unit")]
public class StandardShippingServiceTests
{
    private readonly StandardShippingService _sut;

    public StandardShippingServiceTests()
    {
        ShippingSettings settings = new() { StandardFixedFee = 10.00m };
        IOptions<ShippingSettings> options = Options.Create(settings);

        _sut = new StandardShippingService(options);
    }

    [Fact(DisplayName = "CalculateShippingCost should return fixed fee")]
    public void CalculateShippingCost_ShouldReturnFixedFee_WhenOrderHasItems()
    {
        // Arrange
        Order order = new(customerName: "Test", shippingMethod: ShippingMethod.Standard);

        order.AddItem(new OrderItem(productName: "Item A", price: 10.00m, weightInKg: 0.5m, quantity: 5));

        // Act
        decimal cost = _sut.CalculateShippingCost(order);

        // Assert
        Assert.Equal(10.00m, cost);
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