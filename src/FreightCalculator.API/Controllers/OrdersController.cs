using FreightCalculator.Application.DTOs.Requests;
using FreightCalculator.Application.DTOs.Responses;
using FreightCalculator.Application.UseCases.Orders.Create;
using FreightCalculator.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FreightCalculator.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed partial class OrdersController(ILogger<OrdersController> logger) : ControllerBase
{
    private const int ValidationEventId = 100;
    private const int UnexpectedErrorEventId = 500;

    [HttpPost]
    [ProducesResponseType<OrderProcessedResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public IActionResult Create(ICreateOrderUseCase useCase, CreateOrderRequest request)
    {
        try
        {
            OrderProcessedResponse response = useCase.Execute(request);
            return Ok(response);
        }
        catch (DomainException ex)
        {
            LogValidationFailed(ex.Message);
            return UnprocessableEntity(new { error = ex.Message });
        }
        catch (ValidationException ex)
        {
            LogValidationFailed(ex.Message);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            LogUnexpectedError(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Internal server error." });
        }
    }

    [LoggerMessage(
        EventId = ValidationEventId,
        Level = LogLevel.Warning,
        Message = "Business rule validation failed: {Reason}")]
    private partial void LogValidationFailed(string reason);

    [LoggerMessage(
        EventId = UnexpectedErrorEventId,
        Level = LogLevel.Error,
        Message = "An unexpected error occurred while processing the order")]
    private partial void LogUnexpectedError(Exception ex);
}