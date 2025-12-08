using FreightCalculator.Application.DTOs.Requests;
using FreightCalculator.Application.DTOs.Responses;

namespace FreightCalculator.Application.UseCases.CreateOrder;

public interface ICreateOrderUseCase
{
    OrderProcessedResponse Execute(CreateOrderRequest request);
}