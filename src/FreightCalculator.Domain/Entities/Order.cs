using FreightCalculator.Domain.Common;
using FreightCalculator.Domain.Enums;
using FreightCalculator.Domain.Exceptions;

namespace FreightCalculator.Domain.Entities;

public sealed class Order : IAggregateRoot
{
    public const string CustomerNameCannotBeEmpty = "Customer name cannot be empty.";
    public const string OrderMustHaveItems = "The order must contain at least one item.";

    public Guid Id { get; private set; }
    public string CustomerName { get; private set; }
    public ShippingMethod ShippingMethod { get; private set; }

    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public decimal Total => _items.Sum(i => i.Total);

    public Order(string customerName, ShippingMethod shippingMethod, List<OrderItem> items)
    {
        ValidateDomain(customerName, items);

        Id = Guid.NewGuid();
        CustomerName = customerName;
        ShippingMethod = shippingMethod;

        _items.AddRange(items);
    }

    public void AddItem(OrderItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _items.Add(item);
    }

    private static void ValidateDomain(string customerName, List<OrderItem> items)
    {
        if (string.IsNullOrWhiteSpace(customerName))
        {
            throw new DomainException(CustomerNameCannotBeEmpty);
        }

        if (items is null || items.Count == 0)
        {
            throw new DomainException(OrderMustHaveItems);
        }
    }
}