using FreightCalculator.Domain.Enums;
using System.Text.Json.Serialization;

namespace FreightCalculator.API.DTOs.Requests;

public record CreateOrderRequest(
    string CustomerName,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    [property: JsonRequired]
    ShippingMethod ShippingMethod,
    List<CreateOrderItemRequest> Items);