using FreightCalculator.Domain.Common;
using FreightCalculator.Domain.Entities;

namespace FreightCalculator.Domain.Tests.Entities;

public class OrderItemTests
{
    [Fact(DisplayName = "Total Should Calculate Price Multiplied By Quantity")]
    public void Total_ShouldCalculateCorrectly_WhenValuesAreValid()
    {
        // Arrange
        const decimal price = 10.50m;
        const int quantity = 3;

        // Act
        OrderItem item = new(productName: "Test Product", price: price, weight: 1m, quantity: quantity);

        // Assert
        Assert.Equal(31.50m, item.Total);

        Assert.Equal("Test Product", item.ProductName);
        Assert.Equal(price, item.Price);
        Assert.Equal(1.00m, item.Weight);
        Assert.Equal(quantity, item.Quantity);
        Assert.NotEqual(Guid.Empty, item.Id);
    }

    [Fact(DisplayName = "Constructor Should Throw DomainException When Product Name Is Empty")]
    public void Constructor_ShouldThrowDomainException_WhenProductNameIsEmpty()
    {
        // Act
        static void Act() => _ = new OrderItem(productName: string.Empty, price: 10.00m, weight: 1m, quantity: 1);

        // Assert
        var exception = Assert.Throws<DomainException>(Act);
        Assert.Equal(OrderItem.ProductNameCannotBeEmpty, exception.Message);
    }

    [Theory(DisplayName = "Constructor Should Throw DomainException When Price Is Invalid")]
    [InlineData(0)]
    [InlineData(-10.5)]
    public void Constructor_ShouldThrowDomainException_WhenPriceIsInvalid(decimal invalidPrice)
    {
        // Act
        void Act() => _ = new OrderItem(productName: "Valid Name", price: invalidPrice, weight: 1m, quantity: 1);

        // Assert
        var exception = Assert.Throws<DomainException>(Act);
        Assert.Equal(OrderItem.PriceMustBeGreaterThanZero, exception.Message);
    }

    [Theory(DisplayName = "Constructor Should Throw DomainException When Weight Is Invalid")]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_ShouldThrowDomainException_WhenWeightIsInvalid(decimal invalidWeight)
    {
        // Act
        void Act() => _ = new OrderItem(productName: "Valid Name", price: 10.00m, weight: invalidWeight, quantity: 1);

        // Assert
        var exception = Assert.Throws<DomainException>(Act);
        Assert.Equal(OrderItem.WeightMustBeGreaterThanZero, exception.Message);
    }

    [Theory(DisplayName = "Constructor Should Throw DomainException When Quantity Is Invalid")]
    [InlineData(0)]
    [InlineData(-5)]
    public void Constructor_ShouldThrowDomainException_WhenQuantityIsInvalid(int invalidQuantity)
    {
        // Act
        void Act() => _ = new OrderItem(productName: "Valid Name", price: 10.00m, weight: 1m, quantity: invalidQuantity);

        // Assert
        var exception = Assert.Throws<DomainException>(Act);
        Assert.Equal(OrderItem.QuantityMustBeGreaterThanZero, exception.Message);
    }
}