using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Exceptions;

namespace FreightCalculator.UnitTests.Domain.Entities;

[Trait("Category", "Unit")]
public class OrderItemTests
{
    [Fact(DisplayName = "Total should calculate price multiplied by quantity")]
    public void Total_ShouldCalculateCorrectly_WhenValuesAreValid()
    {
        // Arrange
        const decimal price = 10.50m;
        const int quantity = 3;

        // Act
        OrderItem item = new(productName: "Test Product", price: price, weightInKg: 1m, quantity: quantity);

        // Assert
        Assert.Equal(31.50m, item.Total);

        Assert.Equal("Test Product", item.ProductName);
        Assert.Equal(price, item.Price);
        Assert.Equal(1m, item.WeightInKg);
        Assert.Equal(quantity, item.Quantity);
        Assert.NotEqual(Guid.Empty, item.Id);
    }

    [Fact(DisplayName = "Constructor should throw exception when product name is empty")]
    public void Constructor_ShouldThrowDomainException_WhenProductNameIsEmpty()
    {
        // Act
        static void Act() => _ = new OrderItem(productName: string.Empty, price: 10.00m, weightInKg: 1m, quantity: 1);

        // Assert
        var exception = Assert.Throws<DomainException>(Act);
        Assert.Equal(OrderItem.ProductNameCannotBeEmpty, exception.Message);
    }

    [Theory(DisplayName = "Constructor should throw exception when price is invalid")]
    [InlineData(0)]
    [InlineData(-10.5)]
    public void Constructor_ShouldThrowDomainException_WhenPriceIsInvalid(decimal invalidPrice)
    {
        // Act
        void Act() => _ = new OrderItem(productName: "Valid Name", price: invalidPrice, weightInKg: 1m, quantity: 1);

        // Assert
        var exception = Assert.Throws<DomainException>(Act);
        Assert.Equal(OrderItem.PriceMustBeGreaterThanZero, exception.Message);
    }

    [Theory(DisplayName = "Constructor should throw exception when weight is invalid")]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_ShouldThrowDomainException_WhenWeightIsInvalid(decimal invalidWeight)
    {
        // Act
        void Act() => _ = new OrderItem(productName: "Valid Name", price: 10.00m, weightInKg: invalidWeight, quantity: 1);

        // Assert
        var exception = Assert.Throws<DomainException>(Act);
        Assert.Equal(OrderItem.WeightMustBeGreaterThanZero, exception.Message);
    }

    [Theory(DisplayName = "Constructor should throw exception when quantity is invalid")]
    [InlineData(0)]
    [InlineData(-5)]
    public void Constructor_ShouldThrowDomainException_WhenQuantityIsInvalid(int invalidQuantity)
    {
        // Act
        void Act() => _ = new OrderItem(productName: "Valid Name", price: 10.00m, weightInKg: 1m, quantity: invalidQuantity);

        // Assert
        var exception = Assert.Throws<DomainException>(Act);
        Assert.Equal(OrderItem.QuantityMustBeGreaterThanZero, exception.Message);
    }
}