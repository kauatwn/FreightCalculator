using FreightCalculator.Domain.Common;
using FreightCalculator.Domain.Enums;

namespace FreightCalculator.Domain.Entities;

public class Order : IAggregateRoot
{
    public const string ItemCannotBeNull = "Item cannot be null.";
    public const string CustomerNameCannotBeEmpty = "Customer name cannot be empty.";

    public Guid Id { get; private set; }
    public string CustomerName { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public ShippingMethod ShippingMethod { get; private set; }

    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public decimal Total => _items.Sum(i => i.Total);

    public Order(string customerName, ShippingMethod shippingMethod)
    {
        ValidateDomain(customerName);

        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        CustomerName = customerName;
        ShippingMethod = shippingMethod;
    }

    public void AddItem(OrderItem item)
    {
        if (item is null)
        {
            throw new DomainException(ItemCannotBeNull);
        }

        _items.Add(item);
    }

    private static void ValidateDomain(string customerName)
    {
        if (string.IsNullOrWhiteSpace(customerName))
        {
            throw new DomainException(CustomerNameCannotBeEmpty);
        }
    }
}