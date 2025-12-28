using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Enums;
using FreightCalculator.Domain.Exceptions;

namespace FreightCalculator.UnitTests.Domain.Entities;

[Trait("Category", "Unit")]
public class OrderTests
{
    [Fact(DisplayName = "Constructor should initialize correctly when data is valid")]
    public void Constructor_ShouldInitializeCorrectly_WhenDataIsValid()
    {
        // Arrange
        List<OrderItem> items = [new OrderItem(productName: "Item 1", price: 10m, weightInKg: 1m, quantity: 1)];

        // Act
        Order order = new(customerName: "John Doe", ShippingMethod.Standard, items);

        // Assert
        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.Equal("John Doe", order.CustomerName);
        Assert.Equal(ShippingMethod.Standard, order.ShippingMethod);
        Assert.NotNull(order.Items);
        Assert.Single(order.Items);
        Assert.Equal(10.00m, order.Total);
    }

    [Fact(DisplayName = "Constructor should throw exception when customer name is empty")]
    public void Constructor_ShouldThrowDomainException_WhenCustomerNameIsEmpty()
    {
        // Arrange
        List<OrderItem> items = [new OrderItem(productName: "Item 1", price: 10m, weightInKg: 1m, quantity: 1)];

        // Act
        void Act() => _ = new Order(customerName: string.Empty, ShippingMethod.Standard, items);

        // Assert
        var exception = Assert.Throws<DomainException>(Act);
        Assert.Equal(Order.CustomerNameCannotBeEmpty, exception.Message);
    }

    [Fact(DisplayName = "Constructor should throw exception when items list is empty")]
    public void Constructor_ShouldThrowDomainException_WhenItemsListIsEmpty()
    {
        // Act
        static void Act() => _ = new Order(customerName: "John Doe", shippingMethod: ShippingMethod.Standard, items: []);

        // Assert
        var exception = Assert.Throws<DomainException>(Act);
        Assert.Equal(Order.OrderMustHaveItems, exception.Message);
    }

    [Fact(DisplayName = "Constructor should throw DomainException when items argument is null")]
    public void Constructor_ShouldThrowDomainException_WhenItemsIsNull()
    {
        // Act
        static void Act() => _ = new Order(customerName: "John Doe", ShippingMethod.Standard, items: null!);

        // Assert
        var exception = Assert.Throws<DomainException>(Act);
        Assert.Equal(Order.OrderMustHaveItems, exception.Message);
    }

    [Fact(DisplayName = "AddItem should update total and collection correctly")]
    public void AddItem_ShouldUpdateTotalAndCollection_WhenItemsAreAdded()
    {
        // Arrange
        OrderItem item = new (productName: "Initial Item", price: 10.00m, weightInKg: 1m, quantity: 1);
        Order order = new(customerName: "Client A", shippingMethod: ShippingMethod.Standard, items: [item]);

        OrderItem newItem = new (productName: "New Item", price: 20.00m, weightInKg: 1m, quantity: 2);

        // Act
        order.AddItem(newItem);

        // Assert
        Assert.Equal(2, order.Items.Count);
        Assert.Equal(50.00m, order.Total);
    }

    [Fact(DisplayName = "AddItem should throw exception when item is null")]
    public void AddItem_ShouldThrowArgumentNullException_WhenItemIsNull()
    {
        // Arrange
        List<OrderItem> items = [new OrderItem(productName: "Item 1", price: 10m, weightInKg: 1m, quantity: 1)];
        Order order = new(customerName: "Client A", shippingMethod: ShippingMethod.Standard, items);

        // Act
        void Act() => order.AddItem(null!);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(Act);
        Assert.Equal("item", exception.ParamName);
    }
}