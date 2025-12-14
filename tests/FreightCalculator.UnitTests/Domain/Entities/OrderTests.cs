using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Enums;
using FreightCalculator.Domain.Exceptions;

namespace FreightCalculator.UnitTests.Domain.Entities;

public class OrderTests
{
    [Fact(DisplayName = "Constructor should initialize correctly when data is valid")]
    public void Constructor_ShouldInitializeCorrectly_WhenDataIsValid()
    {
        // Act
        Order order = new(customerName: "John Doe", shippingMethod: ShippingMethod.Standard);

        // Assert
        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.Equal("John Doe", order.CustomerName);
        Assert.Equal(ShippingMethod.Standard, order.ShippingMethod);
        Assert.NotNull(order.Items);
        Assert.Empty(order.Items);
        Assert.Equal(0.00m, order.Total);
    }

    [Fact(DisplayName = "Constructor should throw exception when customer name is empty")]
    public void Constructor_ShouldThrowDomainException_WhenCustomerNameIsEmpty()
    {
        // Act
        static void Act() => _ = new Order(customerName: string.Empty, shippingMethod: ShippingMethod.Standard);

        // Assert
        var exception = Assert.Throws<DomainException>(Act);
        Assert.Equal(Order.CustomerNameCannotBeEmpty, exception.Message);
    }

    [Fact(DisplayName = "AddItem should update total and collection correctly")]
    public void AddItem_ShouldUpdateTotalAndCollection_WhenItemsAreAdded()
    {
        // Arrange
        Order order = new(customerName: "Client A", shippingMethod: ShippingMethod.Standard);

        OrderItem item1 = new(productName: "Item 1", price: 10.00m, weightInKg: 1m, quantity: 1);
        OrderItem item2 = new(productName: "Item 2", price: 20.00m, weightInKg: 1m, quantity: 2);

        // Act
        order.AddItem(item1);
        order.AddItem(item2);

        // Assert
        Assert.Equal(2, order.Items.Count);
        Assert.Equal(50.00m, order.Total);
    }

    [Fact(DisplayName = "AddItem should throw exception when item is null")]
    public void AddItem_ShouldThrowDomainException_WhenItemIsNull()
    {
        // Arrange
        Order order = new(customerName: "Client A", shippingMethod: ShippingMethod.Standard);

        // Act
        void Act() => order.AddItem(null!);

        // Assert
        var exception = Assert.Throws<DomainException>(Act);
        Assert.Equal(Order.ItemCannotBeNull, exception.Message);
    }
}