using FreightCalculator.Domain.Enums;
using FreightCalculator.Domain.Interfaces;
using FreightCalculator.Infrastructure.Services;
using Moq;

namespace FreightCalculator.Infrastructure.Tests.Services;

public class ShippingServiceFactoryTests
{
    private readonly Mock<Func<ShippingMethod, IShippingService>> _mockResolver = new();

    private readonly ShippingServiceFactory _sut;

    public ShippingServiceFactoryTests()
    {
        _sut = new ShippingServiceFactory(_mockResolver.Object);
    }

    [Theory(DisplayName = "GetService Should Return Service When Method Is Valid")]
    [InlineData(ShippingMethod.Standard)]
    [InlineData(ShippingMethod.Express)]
    public void GetService_ShouldReturnService_WhenMethodIsValid(ShippingMethod method)
    {
        // Arrange
        var expected = Mock.Of<IShippingService>();

        _mockResolver.Setup(r => r(method)).Returns(expected);

        // Act
        IShippingService result = _sut.GetService(method);

        // Assert
        Assert.Same(expected, result);

        _mockResolver.Verify(r => r(method), Times.Once);
    }
}