using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Enums;
using FreightCalculator.Domain.Services.Shipping;

namespace FreightCalculator.Domain.Tests.Services.Shipping;

public class StandardShippingServiceTests
{
    private readonly StandardShippingService _sut = new();

    [Fact(DisplayName = "CalculateShippingCost Should Return Fixed Fee")]
    public void CalculateShippingCost_ShouldReturnFixedFee_WhenCalled()
    {
        // Arrange
        Order order = new(customerName: "Test", shippingMethod: ShippingMethod.Standard);

        // Act
        decimal cost = _sut.CalculateShippingCost(order);

        // Assert
        Assert.Equal(10.00m, cost);
    }
}