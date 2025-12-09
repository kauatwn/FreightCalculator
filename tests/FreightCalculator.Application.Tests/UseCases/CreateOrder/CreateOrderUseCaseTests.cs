using FreightCalculator.Application.DTOs.Requests;
using FreightCalculator.Application.DTOs.Responses;
using FreightCalculator.Application.UseCases.CreateOrder;
using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Enums;
using FreightCalculator.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace FreightCalculator.Application.Tests.UseCases.CreateOrder;

public class CreateOrderUseCaseTests
{
    private readonly Mock<IShippingServiceFactory> _mockShippingFactory = new();
    private readonly Mock<IShippingService> _mockShippingService = new();

    private readonly ILogger<CreateOrderUseCase> _logger = Mock.Of<ILogger<CreateOrderUseCase>>();

    private readonly CreateOrderUseCase _sut;

    public CreateOrderUseCaseTests()
    {
        _sut = new CreateOrderUseCase(_mockShippingFactory.Object, _logger);
    }

    [Theory(DisplayName = "Execute should successfully process the order when the request is valid")]
    [InlineData(ShippingMethod.Standard)]
    [InlineData(ShippingMethod.Express)]
    public void Execute_ShouldReturnProcessedOrder_WhenRequestIsValid(ShippingMethod method)
    {
        // Arrange
        CreateOrderRequest request = new(
            CustomerName: "John Doe",
            ShippingMethod: method,
            Items: [new CreateOrderItemRequest(ProductName: "Item 1", Price: 10.00m, WeightInKg: 1m, Quantity: 1)]
        );

        const decimal expectedCost = 15.50m;

        _mockShippingFactory.Setup(f => f.GetService(method))
            .Returns(_mockShippingService.Object);

        _mockShippingService.Setup(s => s.CalculateShippingCost(It.IsAny<Order>()))
            .Returns(expectedCost);

        // Act
        OrderProcessedResponse response = _sut.Execute(request);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(expectedCost, response.ShippingCost);
        Assert.NotEqual(Guid.Empty, response.OrderId);

        _mockShippingFactory.Verify(f => f.GetService(method), Times.Once);

        _mockShippingService.Verify(s => s.CalculateShippingCost(
            It.Is<Order>(o =>
                o.CustomerName == request.CustomerName &&
                o.ShippingMethod == request.ShippingMethod &&
                o.Items.Count == 1)
            ), Times.Once);
    }

    [Fact(DisplayName = "Execute should throw ArgumentException when items list is empty")]
    public void Execute_ShouldThrowArgumentException_WhenItemsListIsEmpty()
    {
        // Arrange
        CreateOrderRequest request = new(
            CustomerName: "John Doe",
            ShippingMethod: ShippingMethod.Standard,
            Items: []
        );

        // Act
        void Act() => _sut.Execute(request);

        // Assert
        var exception = Assert.Throws<ArgumentException>(Act);
        Assert.Equal("request", exception.ParamName);

        _mockShippingFactory.Verify(f => f.GetService(It.IsAny<ShippingMethod>()), Times.Never);
        _mockShippingService.Verify(s => s.CalculateShippingCost(It.IsAny<Order>()), Times.Never);
    }

    [Fact(DisplayName = "Execute should throw ArgumentNullException when request is null")]
    public void Execute_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        // Act
        void Act() => _sut.Execute(null!);

        // Assert
        var exception = Assert.Throws<ArgumentNullException>(Act);
        Assert.Equal("request", exception.ParamName);

        _mockShippingFactory.Verify(f => f.GetService(It.IsAny<ShippingMethod>()), Times.Never);
        _mockShippingService.Verify(s => s.CalculateShippingCost(It.IsAny<Order>()), Times.Never);
    }

    [Fact(DisplayName = "Execute should throw ArgumentException when items list is null")]
    public void Execute_ShouldThrowArgumentException_WhenItemsListIsNull()
    {
        // Arrange
        CreateOrderRequest request = new(
            CustomerName: "John Doe",
            ShippingMethod: ShippingMethod.Standard,
            Items: null!
        );

        // Act
        void Act() => _sut.Execute(request);

        // Assert
        var exception = Assert.Throws<ArgumentException>(Act);
        Assert.Equal("request", exception.ParamName);

        _mockShippingFactory.Verify(f => f.GetService(It.IsAny<ShippingMethod>()), Times.Never);
    }
}