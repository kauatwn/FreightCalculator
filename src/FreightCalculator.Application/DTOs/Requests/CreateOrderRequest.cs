using FreightCalculator.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FreightCalculator.Application.DTOs.Requests;

public sealed record CreateOrderRequest(
    [Required] string CustomerName,
    [Required] ShippingMethod? ShippingMethod,

    [Required]
    [MinLength(1, ErrorMessage = "The order must contain at least one item.")]
    List<CreateOrderItemRequest> Items);