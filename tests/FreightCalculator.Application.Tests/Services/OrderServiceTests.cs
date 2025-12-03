using FreightCalculator.Application.Services;
using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Enums;
using FreightCalculator.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace FreightCalculator.Application.Tests.Services;

public class OrderServiceTests
{
    private readonly Mock<IShippingServiceFactory> _mockShippingFactory = new();
    private readonly Mock<IShippingService> _mockShippingService = new();

    private readonly ILogger<OrderService> _logger = Mock.Of<ILogger<OrderService>>();

    private readonly OrderService _sut;

    public OrderServiceTests()
    {
        _sut = new OrderService(_mockShippingFactory.Object, _logger);
    }

    [Theory(DisplayName = "ProcessOrder Should Request Correct Strategy From Factory")]
    [InlineData(ShippingMethod.Standard)]
    [InlineData(ShippingMethod.Express)]
    public void ProcessOrder_ShouldRequestCorrectStrategy_WhenOrderIsValid(ShippingMethod method)
    {
        // Arrange
        Order order = new(customerName: "John Doe", shippingMethod: method);
        order.AddItem(new OrderItem(productName: "Item 1", price: 10m, weight: 1m, quantity: 1));

        const decimal expectedCost = 10.00m;

        _mockShippingFactory.Setup(f => f.GetService(method))
            .Returns(_mockShippingService.Object);

        _mockShippingService.Setup(s => s.CalculateShippingCost(order))
            .Returns(expectedCost);

        // Act
        _sut.ProcessOrder(order);

        // Assert
        _mockShippingFactory.Verify(f => f.GetService(method), Times.Once);
        _mockShippingService.Verify(s => s.CalculateShippingCost(order), Times.Once);
    }

    [Fact(DisplayName = "ProcessOrder Should Throw ArgumentNullException When Order Is Null")]
    public void ProcessOrder_ShouldThrowArgumentNullException_WhenOrderIsNull()
    {
        // Act
        void Act() => _sut.ProcessOrder(null!);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(Act);
        Assert.Equal("order", exception.ParamName);

        _mockShippingService.Verify(s => s.CalculateShippingCost(It.IsAny<Order>()), Times.Never);
    }
}