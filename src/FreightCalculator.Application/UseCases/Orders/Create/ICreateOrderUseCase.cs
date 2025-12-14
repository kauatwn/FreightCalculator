using FreightCalculator.Application.DTOs.Requests;
using FreightCalculator.Application.DTOs.Responses;

namespace FreightCalculator.Application.UseCases.Orders.Create;

public interface ICreateOrderUseCase
{
    OrderProcessedResponse Execute(CreateOrderRequest request);
}