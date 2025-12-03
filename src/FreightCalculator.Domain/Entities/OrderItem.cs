using FreightCalculator.Domain.Common;

namespace FreightCalculator.Domain.Entities;

public class OrderItem
{
    public const string ProductNameCannotBeEmpty = "Product name cannot be empty.";
    public const string PriceMustBeGreaterThanZero = "Price must be greater than zero.";
    public const string WeightMustBeGreaterThanZero = "Weight must be greater than zero.";
    public const string QuantityMustBeGreaterThanZero = "Quantity must be greater than zero.";

    public Guid Id { get; private set; }
    public string ProductName { get; private set; }
    public decimal Price { get; private set; }
    public decimal Weight { get; private set; }
    public int Quantity { get; private set; }

    public decimal Total => Price * Quantity;

    public OrderItem(string productName, decimal price, decimal weight, int quantity)
    {
        ValidateDomain(productName, price, weight, quantity);

        Id = Guid.NewGuid();
        ProductName = productName;
        Price = price;
        Weight = weight;
        Quantity = quantity;
    }

    private static void ValidateDomain(string productName, decimal price, decimal weight, int quantity)
    {
        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new DomainException(ProductNameCannotBeEmpty);
        }

        if (price <= 0)
        {
            throw new DomainException(PriceMustBeGreaterThanZero);
        }

        if (weight <= 0)
        {
            throw new DomainException(WeightMustBeGreaterThanZero);
        }

        if (quantity <= 0)
        {
            throw new DomainException(QuantityMustBeGreaterThanZero);
        }
    }
}