using FreightCalculator.Application.DTOs.Requests;
using FreightCalculator.Application.DTOs.Responses;
using FreightCalculator.Application.UseCases.CreateOrder;
using FreightCalculator.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace FreightCalculator.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public partial class OrdersController(ILogger<OrdersController> logger) : ControllerBase
{
    private const int ValidationEventId = 100;
    private const int UnexpectedErrorEventId = 500;

    [HttpPost]
    [ProducesResponseType<OrderProcessedResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create(ICreateOrderUseCase useCase, CreateOrderRequest request)
    {
        try
        {
            OrderProcessedResponse response = useCase.Execute(request);
            return Ok(response);
        }
        catch (Exception ex) when (ex is DomainException or ArgumentException)
        {
            LogValidationFailed(ex.Message);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            LogUnexpectedError(ex);
            return StatusCode(500, new { error = "Internal server error." });
        }
    }

    [LoggerMessage(EventId = ValidationEventId, Level = LogLevel.Warning, Message = "Validation failed: {Reason}")]
    private partial void LogValidationFailed(string reason);

    [LoggerMessage(EventId = UnexpectedErrorEventId, Level = LogLevel.Error, Message = "An unexpected error occurred while processing the order")]
    private partial void LogUnexpectedError(Exception ex);
}