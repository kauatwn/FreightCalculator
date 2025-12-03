using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Enums;
using FreightCalculator.Domain.Services.Shipping;

namespace FreightCalculator.Domain.Tests.Services.Shipping;

public class ExpressShippingServiceTests
{
    private readonly ExpressShippingService _sut = new();

    [Fact(DisplayName = "CalculateShippingCost Should Calculate Based On Weight")]
    public void CalculateShippingCost_ShouldCalculateBasedOnWeight_WhenOrderHasItems()
    {
        // Arrange
        Order order = new(customerName: "Test", shippingMethod: ShippingMethod.Express);

        order.AddItem(new OrderItem(productName: "Item A", price: 10m, weight: 2m, quantity: 1));
        order.AddItem(new OrderItem(productName: "Item B", price: 10m, weight: 3m, quantity: 1));

        // Act
        decimal cost = _sut.CalculateShippingCost(order);

        // Assert
        Assert.Equal(5.00m, cost);
    }

    [Fact(DisplayName = "CalculateShippingCost Should Return Zero When Order Is Empty")]
    public void CalculateShippingCost_ShouldReturnZero_WhenOrderHasNoItems()
    {
        // Arrange
        Order order = new(customerName: "Test", shippingMethod: ShippingMethod.Express);

        // Act
        decimal cost = _sut.CalculateShippingCost(order);

        // Assert
        Assert.Equal(0m, cost);
    }
}