using FreightCalculator.API.DTOs.Requests;
using FreightCalculator.API.DTOs.Responses;
using FreightCalculator.Domain.Common;
using FreightCalculator.Domain.Entities;
using FreightCalculator.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FreightCalculator.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<OrderProcessedResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create(IOrderService orderService, [FromBody] CreateOrderRequest request)
    {
        try
        {
            Order order = new(request.CustomerName, request.ShippingMethod);

            foreach (CreateOrderItemRequest item in request.Items)
            {
                order.AddItem(new OrderItem(item.ProductName, item.Price, item.Weight, item.Quantity));
            }

            orderService.ProcessOrder(order);

            OrderProcessedResponse response = new(order.Id);
            return Ok(response);
        }
        catch (DomainException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { error = "Internal server error." });
        }
    }
}